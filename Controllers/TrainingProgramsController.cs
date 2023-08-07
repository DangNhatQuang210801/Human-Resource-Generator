using Microsoft.AspNetCore.Mvc;
using Human_Resource_Generator.Models;
using Human_Resource_Generator.Repository;
using Human_Resource_Generator.ViewModels.TrainingProgramViewModel;
using Newtonsoft.Json;
using AutoMapper;
using Human_Resource_Generator.ViewModels.AttendanceViewModels;
using Human_Resource_Generator.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Human_Resource_Generator.Utility;
using OfficeOpenXml;

namespace Human_Resource_Generator.Controllers
{
    public class TrainingProgramsController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ITrainingProgramRepository _trainingProgramRepository;
        private readonly IEmployeeTrainingRepository _employeeTrainingRepository;
        private readonly IAttendanceRepository _attendanceRepository;
        private readonly IAttendanceEmployeeRepository _attendanceEmployeeRepository;
        private readonly IEmployeeRepo _employeeRepo;

        public TrainingProgramsController(IMapper mapper, ITrainingProgramRepository trainingProgramRepository,
            IEmployeeTrainingRepository employeeTrainingRepository, IAttendanceRepository attendanceRepository,
            IAttendanceEmployeeRepository attendanceEmployeeRepository, IEmployeeRepo employeeRepo)
        {
            _trainingProgramRepository = trainingProgramRepository;
            _employeeTrainingRepository = employeeTrainingRepository;
            _mapper = mapper;
            _attendanceRepository = attendanceRepository;
            _attendanceEmployeeRepository = attendanceEmployeeRepository;
            _employeeRepo = employeeRepo;
        }

        // GET: TrainingPrograms
        public IActionResult Index(string name)
        {
            var model = _trainingProgramRepository.GetAllByFilter(name);
            int totalEmployee = _employeeRepo.CountAllEmployee();
            ViewBag.totalEmployee = totalEmployee.ToString();
            return View(model);
        }

        // GET: TrainingPrograms/Details/5
        public IActionResult Details(int id)
        {
            var trainingProgram = _trainingProgramRepository.GetById(id);
            if (trainingProgram == null)
            {
                return NotFound();
            }

            var employeeTrainingsJoined = _employeeTrainingRepository.GetByTrainingProgramId(id);
            var employeeTrainingIdsJoinded = new List<int>();
            var employeesJoined = new List<Employee>();
            if (employeeTrainingsJoined != null)
            {
                employeeTrainingsJoined.ForEach(e => { employeeTrainingIdsJoinded.Add(e.EmployeeId); });
            }

            employeesJoined = _employeeTrainingRepository.GetListDataByListId(employeeTrainingIdsJoinded);
            DetailTrainingProgramViewModel model = _mapper.Map<DetailTrainingProgramViewModel>(trainingProgram);
            model.JoinedEmployees = employeesJoined;

            return View(model);
        }

        // GET: TrainingPrograms/Create
        [HttpGet]
        public IActionResult Create()
        {
            var model = new CreateTrainingProgramViewModel();
            model.Employees = _employeeTrainingRepository.GetAll().ToList();
            return View(model);
        }

        public ActionResult RenderEmployee(RenderEmployeeViewModel renderEmployeeViewModel)
        {
            var employees = new List<Employee>();
            if (string.IsNullOrEmpty(renderEmployeeViewModel.Searching))
            {
                employees = _employeeRepo.GetAll().ToList();
            }
            else
            {
                employees = _employeeRepo.GetEmployeesByName(renderEmployeeViewModel.Searching);
            }

            TempData["employeeIds"] = renderEmployeeViewModel.CheckedEmployeeIds;

            return PartialView("_Employees", employees);
        }

        // POST: TrainingPrograms/Create
        [HttpPost]
        public async Task<ActionResult> Create(InputTrainingProgramViewModel inputTrainingProgram)
        {
            inputTrainingProgram.UpdatedAt = DateTime.Now;
            List<string> employeeIdsString =
                JsonConvert.DeserializeObject<List<string>>(inputTrainingProgram.EmployeeIds);
            List<int> employeeIds = employeeIdsString.Select(int.Parse).ToList();
            var newTrainingProgramId = await _trainingProgramRepository.Add(inputTrainingProgram);

            if (newTrainingProgramId == -1)
            {
                return Json(new
                    { redirectToUrl = "", statusCode = 502, message = "This Training Program is already existed!" });
            }

            foreach (var employeeId in employeeIds)
            {
                _employeeTrainingRepository.Add(new EmployeeTraining()
                    { EmployeeId = employeeId, TrainingProgramId = newTrainingProgramId });
            }

            return Json(new
                { redirectToUrl = Url.Action("Index", "TrainingPrograms"), statusCode = 200, message = "" });
        }

        // GET: TrainingPrograms/Edit/5
        public IActionResult Edit(int id)
        {
            var trainingProgram = _trainingProgramRepository.GetById(id);
            if (trainingProgram == null)
            {
                return NotFound();
            }

            var employeeTrainingsJoined = _employeeTrainingRepository.GetByTrainingProgramId(id);
            var employeeTrainingIdsJoinded = new List<int>();
            if (employeeTrainingsJoined != null)
            {
                employeeTrainingsJoined.ForEach(e => { employeeTrainingIdsJoinded.Add(e.EmployeeId); });
            }

            EditTrainingProgramViewModel model = _mapper.Map<EditTrainingProgramViewModel>(trainingProgram);
            model.JoinedEmployeeIds = employeeTrainingIdsJoinded;
            model.Employees = _employeeTrainingRepository.GetAll().ToList();

            return View(model);
        }

        // POST: TrainingPrograms/Edit/5
        [HttpPost]
        public ActionResult Edit(InputTrainingProgramViewModel inputTrainingProgram)
        {
            inputTrainingProgram.UpdatedAt = DateTime.Now;
            List<string> employeeIdsString =
                JsonConvert.DeserializeObject<List<string>>(inputTrainingProgram.EmployeeIds);
            List<int> employeeIds = employeeIdsString.Select(int.Parse).ToList();
            _trainingProgramRepository.Update(inputTrainingProgram);


            //Get list old EmployeeTraining
            var oldEmployeeTrainingsJoined =
                _employeeTrainingRepository.GetByTrainingProgramId(inputTrainingProgram.Id);
            var oldEmployeeTrainingIdsJoinded = new List<int>();
            if (oldEmployeeTrainingsJoined != null)
            {
                oldEmployeeTrainingsJoined.ForEach(e => { oldEmployeeTrainingIdsJoinded.Add(e.EmployeeId); });
            }

            var listRemovedEmployeeId = oldEmployeeTrainingIdsJoinded.Except(employeeIds).ToList();
            var listAttendanceByProgramId = _attendanceRepository.GetAllByTrainingProgramId(inputTrainingProgram.Id);
            if (listRemovedEmployeeId != null)
            {
                listRemovedEmployeeId.ForEach(e =>
                {
                    _employeeTrainingRepository.DeleteByTrainingProgramIdAndEmployeeId(inputTrainingProgram.Id, e);
                    //delete attendace related employee with trainingProgram
                    listAttendanceByProgramId.ForEach(att =>
                    {
                        _attendanceEmployeeRepository.DeleteByAttendanceIdAndEmployeeId(att.Id, e);
                    });
                });
            }

            foreach (var employeeId in employeeIds)
            {
                var existedEmployeeTraining =
                    _employeeTrainingRepository.GetByTrainingProgramIdAndEmployeeId(inputTrainingProgram.Id,
                        employeeId);
                if (existedEmployeeTraining == null)
                {
                    _employeeTrainingRepository.Add(new EmployeeTraining()
                        { EmployeeId = employeeId, TrainingProgramId = inputTrainingProgram.Id });
                }
            }

            return Json(new
                { redirectToUrl = Url.Action("Index", "TrainingPrograms"), statusCode = 200, message = "" });
        }

        // [Authorize(Roles =SD.Role_Admin)]
        // GET: TrainingPrograms/Delete/5
        public IActionResult Delete(int id)
        {
            var trainingProgram = _trainingProgramRepository.GetById(id);
            if (trainingProgram == null)
            {
                return NotFound();
            }

            return View(trainingProgram);
        }

        // POST: TrainingPrograms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var trainingProgram = _trainingProgramRepository.GetById(id);
            if (trainingProgram == null)
            {
                return NotFound();
            }

            if (trainingProgram != null)
            {
                _employeeTrainingRepository.DeleteByTrainingProgramId(trainingProgram.Id);
                _trainingProgramRepository.Delete(trainingProgram);
            }

            return RedirectToAction(nameof(Index));
        }


        // GET: TrainingPrograms/Attendance/5
        [HttpGet]
        public IActionResult Attendance(int id)
        {
            var trainingProgram = _trainingProgramRepository.GetById(id);
            if (trainingProgram == null)
            {
                return NotFound();
            }

            var employeeTrainingsJoined = _employeeTrainingRepository.GetByTrainingProgramId(id);
            var employeeTrainingIdsJoinded = new List<int>();
            var employeesJoined = new List<Employee>();
            if (employeeTrainingsJoined != null)
            {
                employeeTrainingsJoined.ForEach(e => { employeeTrainingIdsJoinded.Add(e.EmployeeId); });
            }

            employeesJoined = _employeeRepo.GetListDataByListId(employeeTrainingIdsJoinded);
            AttendanceViewModel model = _mapper.Map<AttendanceViewModel>(trainingProgram);
            model.JoinedEmployees = employeesJoined;
            model.Attendances = _attendanceRepository.GetAllByTrainingProgramId(id);
            var attendances = _attendanceRepository.GetAllByTrainingProgramId(id);
            var listEmployeeWithDate = new List<JoinedEmployeeWithDateViewModel>();
            attendances.ForEach(e =>
            {
                if (e.AttendanceEmployees.Count > 0)
                {
                    foreach (var i in e.AttendanceEmployees)
                    {
                        listEmployeeWithDate.Add(new JoinedEmployeeWithDateViewModel()
                        {
                            JoinedDate = e.AttendanceDate,
                            EmployeeId = i.EmployeeId,
                            AttendanceAt = i.AttendanceAt,
                        });
                    }
                }
            });
            model.JoinedEmployeeWithDate = listEmployeeWithDate;

            int totalEmployee = employeesJoined.Count;

            ViewBag.totalEmployee = totalEmployee.ToString();

            return View(model);
        }

        // GET: TrainingPrograms/CreateAttendance/id
        [HttpGet]
        public IActionResult CreateAttendance(int id)
        {
            var trainingProgram = _trainingProgramRepository.GetById(id);
            if (trainingProgram == null)
            {
                return NotFound();
            }

            var employeeTrainingsJoined = _employeeTrainingRepository.GetByTrainingProgramId(id);
            var employeeTrainingIdsJoinded = new List<int>();
            List<Employee> employeesJoined = new List<Employee>();
            if (employeeTrainingsJoined != null)
            {
                employeeTrainingsJoined.ForEach(e => { employeeTrainingIdsJoinded.Add(e.EmployeeId); });
            }

            employeesJoined = _employeeTrainingRepository.GetListDataByListId(employeeTrainingIdsJoinded);
            DetailTrainingProgramViewModel model = _mapper.Map<DetailTrainingProgramViewModel>(trainingProgram);
            model.JoinedEmployees = employeesJoined;
            return View(model);
        }

        // POST: TrainingPrograms/CreateAttendance
        [HttpPost]
        public IActionResult CreateAttendance(InputAttendanceViewModel input)
        {
            List<EmployeeIdWithScore> employeeIdWithScoreString =
                JsonConvert.DeserializeObject<List<EmployeeIdWithScore>>(input.ListEmployeeIdWithScore);

            // Create new attendance for this training program
            var newAttendanceId = _attendanceRepository.Add(new Attendance()
                { AttendanceDate = input.AttendanceDate, TrainingProgramId = input.TrainingProgramId });
            if (newAttendanceId != -1)
            {
                //Create new attendance for all joined employees
                employeeIdWithScoreString.ForEach(e =>
                {
                    _attendanceEmployeeRepository.Add(new AttendanceEmployee()
                    {
                        AttendanceId = newAttendanceId,
                        EmployeeId = Int32.Parse(e.EmployeeId),
                        Score = Int32.Parse(e.Score),
                        AttendanceAt = DateTime.Now,
                    });
                });
            }
            else
            {
                return Json(new
                    { redirectToUrl = "", statusCode = 502, message = "This Attendance day is already existed!" });
            }

            return Json(new
            {
                redirectToUrl = Url.Action("Attendance", "TrainingPrograms", new { id = input.TrainingProgramId }),
                statusCode = 200, message = ""
            });
        }

        // GET: TrainingPrograms/UpdateAttendance/id
        [HttpGet]
        public IActionResult UpdateAttendance(int id)
        {
            var attendance = _attendanceRepository.GetById(id);
            if (attendance == null)
            {
                return NotFound();
            }

            var trainingProgram = _trainingProgramRepository.GetById(attendance.TrainingProgramId);
            if (trainingProgram == null)
            {
                return NotFound();
            }

            var employeeTrainingsJoined = _employeeTrainingRepository.GetByTrainingProgramId(trainingProgram.Id);
            var employeeTrainingIdsJoinded = new List<int>();
            List<Employee> employeesJoined = new List<Employee>();
            if (employeeTrainingsJoined != null)
            {
                employeeTrainingsJoined.ForEach(e => { employeeTrainingIdsJoinded.Add(e.EmployeeId); });
            }

            employeesJoined = _employeeTrainingRepository.GetListDataByListId(employeeTrainingIdsJoinded);
            EditAttendanceViewModel model = _mapper.Map<EditAttendanceViewModel>(trainingProgram);
            model.Attendance = attendance;
            model.JoinedEmployees = employeesJoined;
            model.AttendanceEmployees = attendance.AttendanceEmployees.ToList();

            return View(model);
        }

        // POST: TrainingPrograms/UpdateAttendance
        [HttpPost]
        public IActionResult UpdateAttendance(InputUpdateAttendanceViewModel input)
        {
            // Update Attendance
            var currentAttendance = _attendanceRepository.GetById(input.Id);
            currentAttendance.AttendanceDate = input.AttendanceDate;
            _attendanceRepository.Update(currentAttendance);
            List<EmployeeIdWithScore> employeeIdWithScoreString =
                JsonConvert.DeserializeObject<List<EmployeeIdWithScore>>(input.ListEmployeeIdWithScore);

            //Get old list EmployeeAttendance to check if this request have deleting employee attendance
            List<AttendanceEmployee> oldAttendanceEmployees = _attendanceEmployeeRepository.GetByAttendanceId(input.Id);

            // Get list old EmployeeIds, new EmployeeIds => compare to get list deleted EmployeeId
            var listOldEmployeeIds = new List<int>();
            oldAttendanceEmployees.ForEach(oldE => { listOldEmployeeIds.Add(oldE.EmployeeId); });
            var listNewEmployeeIds = new List<int>();
            employeeIdWithScoreString.ForEach(newE => { listNewEmployeeIds.Add(Int32.Parse(newE.EmployeeId)); });

            var listEmployeeIdDeleted = new List<int>();
            listOldEmployeeIds.ForEach(oldE =>
            {
                if (!listNewEmployeeIds.Contains(oldE))
                {
                    listEmployeeIdDeleted.Add(oldE);
                }
            });

            //Delete AttendanceEmployee of employee deleted
            listEmployeeIdDeleted.ForEach(emId =>
            {
                _attendanceEmployeeRepository.DeleteByAttendanceIdAndEmployeeId(input.Id, emId);
            });

            //Create/Update attendance for all joined employees
            employeeIdWithScoreString.ForEach(e =>
            {
                _attendanceEmployeeRepository.Add(new AttendanceEmployee()
                {
                    AttendanceId = input.Id,
                    EmployeeId = Int32.Parse(e.EmployeeId),
                    Score = Int32.Parse(e.Score),
                    AttendanceAt = DateTime.Now,
                });
            });

            return Json(new
            {
                redirectToUrl = Url.Action("Attendance", "TrainingPrograms", new { id = input.TrainingProgramId }),
                statusCode = 200, message = ""
            });
        }

        // POST: TrainingPrograms/DeleteAttendance
        [HttpPost]
        public ActionResult DeleteAttendance(int id, int programId)
        {
            var attendance = _attendanceRepository.GetById(id);
            _attendanceRepository.Delete(attendance);
            return Json(new
            {
                redirectToUrl = Url.Action("Attendance", "TrainingPrograms", new { id = programId }), statusCode = 200,
                message = ""
            });
        }

        [HttpGet]
        public IActionResult CreateAttendanceByScanner(int id)
        {
            return View(id);
        }

        [HttpPost]
        public IActionResult CreateAttendanceByScanner(string listCodes, int trainingProgramId)
        {
            var listEmployeeCode = JsonConvert.DeserializeObject<List<string>>(listCodes);
            var listEmployeeId = new List<int>();
            listEmployeeCode?.ForEach(c =>
            {
                var employeeId = _employeeRepo.GetEmployeeIdByCodeScanner(c);
                if (employeeId != null)
                {
                    listEmployeeId.Add(employeeId ?? 1);
                }
            });
            // Create new attendance for this training program
            var newAttendanceId = _attendanceRepository.Add(new Attendance()
                { AttendanceDate = DateTime.Now, TrainingProgramId = trainingProgramId });
            if (newAttendanceId != -1)
            {
                //Create new attendance for all joined employees
                listEmployeeId.ForEach(eId =>
                {
                    _attendanceEmployeeRepository.Add(new AttendanceEmployee()
                    {
                        AttendanceId = newAttendanceId,
                        EmployeeId = eId,
                        Score = 0,
                        AttendanceAt = DateTime.Now,
                    });
                });
            }
            else
            {
                return Json(new
                    { redirectToUrl = "", statusCode = 502, message = "This Attendance day is already existed!" });
            }

            return Json(new
            {
                redirectToUrl = Url.Action("Attendance", "TrainingPrograms", new { id = trainingProgramId }),
                statusCode = 200, message = ""
            });
        }

        [HttpPost]
        public ActionResult Import(IFormFile? file)
        {
            var excelDataList = new List<string>();
            if (file is not { Length: > 0 }) 
                return PartialView("_Employees", new List<Employee>());
            if (file.Length <= 0)
            {
                return PartialView("_Employees", new List<Employee>());
            }
            using var stream = new MemoryStream();
            file.CopyTo(stream);
            using var package = new ExcelPackage(stream);
            var worksheet = package.Workbook.Worksheets[0]; // Assuming the data is in the first worksheet
            var rowCount = worksheet.Dimension.Rows;

            for (var row = 2;
                 row <= rowCount;
                 row++) 
            {
                var value = worksheet?.Cells[row, 1]?.Value?.ToString() ?? "0";
                excelDataList.Add(value);
            }

            var listEmployees = _employeeRepo.GetEmployeesByListCodes(excelDataList);
            return PartialView("_Employees", listEmployees);
        }
        
        [HttpPost]
        public List<string> ImportAttendance(IFormFile? file)
        {
            var excelDataList = new List<string>();
            if (file is not { Length: > 0 })
                return excelDataList;
            if (file.Length <= 0)
            {
                return excelDataList;
            }
            using var stream = new MemoryStream();
            file.CopyTo(stream);
            using var package = new ExcelPackage(stream);
            var worksheet = package.Workbook.Worksheets[0]; // Assuming the data is in the first worksheet
            var rowCount = worksheet.Dimension.Rows;

            for (var row = 2;
                 row <= rowCount;
                 row++) 
            {
                var code = worksheet?.Cells[row, 1]?.Value?.ToString() ?? "0";
                var score = worksheet?.Cells[row, 3]?.Value?.ToString() ?? "0";
                excelDataList.Add(code+":"+score);
            }

            return excelDataList;
        }
    }
}
