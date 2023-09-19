using Microsoft.AspNetCore.Mvc;
using Human_Resource_Generator.Models;
using Human_Resource_Generator.Repository;
using Human_Resource_Generator.ViewModels.AttendanceViewModels;
using Human_Resource_Generator.ViewModels.TrainingProgramViewModel;
using Newtonsoft.Json;
using OfficeOpenXml;
using ClosedXML.Excel;
using Microsoft.Extensions.Caching.Memory;
using AutoMapper;
using Human_Resource_Generator.Interfaces;

namespace Human_Resource_Generator.Controllers
{
    public class TrainingProgramsController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;
        private readonly ITrainingProgramRepository _trainingProgramRepository;
        private readonly IEmployeeTrainingRepository _employeeTrainingRepository;
        private readonly IAttendanceRepository _attendanceRepository;
        private readonly IAttendanceEmployeeRepository _attendanceEmployeeRepository;
        private readonly IEmployeeRepo _employeeRepo;
        private readonly ITrainingProgramRepository _GetAllEmployeesByTrainingProgramId;

        public TrainingProgramsController(IMapper mapper, ITrainingProgramRepository trainingProgramRepository,
            IEmployeeTrainingRepository employeeTrainingRepository, IAttendanceRepository attendanceRepository,
            IAttendanceEmployeeRepository attendanceEmployeeRepository, IEmployeeRepo employeeRepo,
            ITrainingProgramRepository GetAllEmployeesByTrainingProgramId, IMemoryCache memoryCache)
        {
            _trainingProgramRepository = trainingProgramRepository;
            _employeeTrainingRepository = employeeTrainingRepository;
            _mapper = mapper;
            _attendanceRepository = attendanceRepository;
            _attendanceEmployeeRepository = attendanceEmployeeRepository;
            _employeeRepo = employeeRepo;
            _GetAllEmployeesByTrainingProgramId = GetAllEmployeesByTrainingProgramId;
            _memoryCache = memoryCache;
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
                statusCode = 200,
                message = ""
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
                statusCode = 200,
                message = ""
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
                redirectToUrl = Url.Action("Attendance", "TrainingPrograms", new { id = programId }),
                statusCode = 200,
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
                statusCode = 200,
                message = ""
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
                excelDataList.Add(code + ":" + score);
            }

            return excelDataList;
        }

        [HttpPost]
        public IActionResult DataDownloadAttendance([FromBody] List<DataDownloadAttendanceViewModel> data)
        {
            var token = Guid.NewGuid().ToString();
            _memoryCache.Set(token, data);
            return Ok(token);
        }
        [HttpGet]
        public IActionResult DownloadFormTemplate(int id)
        {
            var employees = _trainingProgramRepository.GetAllEmployeesByTrainingProgramId(id);
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Form Template");

                // Set column headers and width
                worksheet.Cell(1, 1).Value = "Code";
                worksheet.Cell(1, 2).Value = "Name";
                worksheet.Cell(1, 3).Value = "Score";
                worksheet.Column(1).Width = 15;
                worksheet.Column(2).Width = 20;
                worksheet.Column(3).Width = 15;

                // Populate data
                int row = 2;
                foreach (var employee in employees)
                {
                    worksheet.Cell(row, 1).Value = employee.Code;
                    worksheet.Cell(row, 2).Value = employee.Name;

                    row++;
                }

                // Save the workbook to a MemoryStream
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Seek(0, SeekOrigin.Begin);

                    // Return the Excel file for download
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "FormTemplate.xlsx");
                }
            }
        }

        [HttpGet]
        public IActionResult ExportFormForCreateTraining()
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Empty Code List");

                worksheet.Cell(1, 1).Value = "Code";
                worksheet.Column(1).Width = 15;

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Seek(0, SeekOrigin.Begin);

                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "EmptyCodeList.xlsx");
                }
            }
        }

        [HttpGet]
        public IActionResult ExportAttendance(string token)
        {
            var data = _memoryCache.Get<List<DataDownloadAttendanceViewModel>>(token);
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Training Program Attendance");

                // First row contain text "付属書 No. SEDV-M100-0001-6 (10) 1/1"
                worksheet.Cell("I1").Value = "付属書 No. SEDV-M100-0001-6 (10) 1/1";
                worksheet.Range("I1:L2").Row(1).Merge();
                worksheet.Cell("I1").Style.Font.SetFontSize(10);

                // Second row
                worksheet.Cell("A2").Value = "BẢNG GHI CHÉP HƯỚNG DẪN 教育記録";
                worksheet.Cell("A2").Style.Font.SetFontSize(20);
                worksheet.Cell("A2").Style.Font.SetBold();
                worksheet.Range("A2:L3").Row(1).Merge();

                // Third row
                worksheet.Cell("A3").Value = $"\u25cbThời gian  日時  {data.ElementAt(0).CreateAt}";
                worksheet.Cell("A3").Style.Font.SetFontSize(12);
                worksheet.Range("A3:L4").Row(1).Merge();

                // fourth row
                worksheet.Cell("A4").Value =
                    $"\u25cbHạng mục 項目 : {data.ElementAt(0).Subject}";
                worksheet.Cell("A4").Style.Font.SetFontSize(12);
                worksheet.Range("A4:L5").Row(1).Merge();

                // fifth row
                worksheet.Cell("A5").Value = $"\u25cbNgười hướng dẫn 講師 \t\t   {data.ElementAt(0).Teacher}";
                worksheet.Cell("A5").Style.Font.SetFontSize(12);
                worksheet.Range("A5:L6").Row(1).Merge();

                // sixth row
                worksheet.Cell("A6").Value =
                    "\u25cbTài liệu hướng dẫn đính kèm  概要 ；使用教育資料 添付 0Có有   (Ghi rõ tên + Mã số ở ô bên dưới)    0Không 無 (Ghi rõ nội dung  hướng dẫn ở ô bên dưới)";
                worksheet.Cell("A6").Style.Font.SetFontSize(12);
                worksheet.Cell("A6").Style.Fill.SetBackgroundColor(XLColor.Gray);
                worksheet.Range("A6:L7").Row(1).Merge();

                // seventh row
                worksheet.Cell("A7").Value = "Tài liệu đào tạo SEDV-HR&GA-23-02";
                worksheet.Cell("A7").Style.Font.SetFontSize(12);
                worksheet.Range("A7:L8").Row(1).Merge();
                worksheet.Cell("A7").WorksheetRow().Height = 50;
                
                // eighth row
                worksheet.Cell("A8").Style.Fill.SetBackgroundColor(XLColor.Gray);
                worksheet.Range("A8:L9").Row(1).Merge();
                
                // ninth row
                worksheet.Range("A9:L9").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                worksheet.Range("A9:L9").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                worksheet.Cell("A9").Value = "No.";
                worksheet.Cell("B9").Value = "Số thẻ\n所属";
                worksheet.Cell("C9").Value = "Tên\n氏名";
                worksheet.Cell("C9").WorksheetColumn().Width = 30;
                worksheet.Cell("D9").Value = "Xác nhận tham gia\n確認";
                worksheet.Cell("E9").Value = "Mức độ hiểu\n理解度";
                worksheet.Cell("F9").Value = "Ghi chú\n備考";
                
                worksheet.Cell("G9").Value = "No.";
                worksheet.Cell("H9").Value = "Số thẻ\n所属";
                worksheet.Cell("I9").Value = "Tên\n氏名";
                worksheet.Cell("I9").WorksheetColumn().Width = 30;
                worksheet.Cell("J9").Value = "Xác nhận tham gia\n確認";
                worksheet.Cell("K9").Value = "Mức độ hiểu\n理解度";
                worksheet.Cell("L9").Value = "Ghi chú\n備考";
                
                // Insert data
                var count = data.Count;
                var mid = (count + 1) / 2;
                for (var i = 0; i < mid; i++)
                {
                    worksheet.Cell($"A{10 + i}").Value = i;
                    worksheet.Cell($"B{10 + i}").Value = data.ElementAt(i).Code;
                    worksheet.Cell($"C{10 + i}").Value = data.ElementAt(i).Name;
                    worksheet.Cell($"E{10 + i}").Value = data.ElementAt(i).Score;
                }
                for (var i = mid; i < count; i++)
                {
                    worksheet.Cell($"G{10 + i - mid}").Value = i ;
                    worksheet.Cell($"H{10 + i - mid}").Value = data.ElementAt(i).Code;
                    worksheet.Cell($"I{10 + i - mid}").Value = data.ElementAt(i).Name;
                    worksheet.Cell($"K{10 + i - mid}").Value = data.ElementAt(i).Score;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Seek(0, SeekOrigin.Begin);

                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "EmptyCodeList.xlsx");
                }
            }
        }
    }
}

