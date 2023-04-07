using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Human_Resource_Generator.Data;
using Human_Resource_Generator.Models;
using Human_Resource_Generator.Repository;
using Human_Resource_Generator.ViewModels.TrainingProgramViewModel;
using Newtonsoft.Json;
using AutoMapper;
using AutoMapper.Internal.Mappers;
using Human_Resource_Generator.Repository.Implement;
using System.Collections;
using Human_Resource_Generator.ViewModels.AttendanceViewModels;

namespace Human_Resource_Generator.Controllers
{
    public class TrainingProgramsController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ITrainingProgramRepository _trainingProgramRepository;
        private readonly IEmployeeTrainingRepository _employeeTrainingRepository;
        private readonly IAttendanceRepository _attendanceRepository;
        private readonly IAttendanceEmployeeRepository _attendanceEmployeeRepository;

        public TrainingProgramsController(IMapper mapper, ITrainingProgramRepository trainingProgramRepository, IEmployeeTrainingRepository employeeTrainingRepository, IAttendanceRepository attendanceRepository, IAttendanceEmployeeRepository attendanceEmployeeRepository)
        {
            _trainingProgramRepository = trainingProgramRepository;
            _employeeTrainingRepository = employeeTrainingRepository;
            _mapper = mapper;
            _attendanceRepository = attendanceRepository;
            _attendanceEmployeeRepository = attendanceEmployeeRepository;
        }

        // GET: TrainingPrograms
        public IActionResult Index(string name)
        {
            var model = _trainingProgramRepository.GetAllByFilter(name);
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
                employeeTrainingsJoined.ForEach(e =>
                {
                    employeeTrainingIdsJoinded.Add(e.EmployeeId);
                });
            }
            employeesJoined = _employeeTrainingRepository.GetListDataByListId(employeeTrainingIdsJoinded);
            DetailTrainingProgramViewModel model = _mapper.Map<DetailTrainingProgramViewModel>(trainingProgram);
            model.JoinedEmployees = employeesJoined;

            return View(model);
        }

        // GET: TrainingPrograms/Create
        public IActionResult Create()
        {
            var model = new CreateTrainingProgramViewModel();
            model.Employees = _employeeTrainingRepository.GetAll().ToList();
            return View(model);
        }

        // POST: TrainingPrograms/Create
        [HttpPost]
        public ActionResult Create(InputTrainingProgramViewModel inputTrainingProgram)
        {
            inputTrainingProgram.CreatedAt = DateTime.Now;
            inputTrainingProgram.UpdatedAt = DateTime.Now;
            List<string> employeeIdsString = JsonConvert.DeserializeObject<List<string>>(inputTrainingProgram.EmployeeIds);
            List<int> employeeIds = employeeIdsString.Select(int.Parse).ToList();
            var newTrainingProgramId = _trainingProgramRepository.Add(inputTrainingProgram);
            if (newTrainingProgramId == -1)
            {
                return Json(new { redirectToUrl = "", statusCode = 502, message = "This Training Program is already existed!" });
            }
            foreach (var employeeId in employeeIds)
            {
                _employeeTrainingRepository.Add(new EmployeeTraining() { EmployeeId = employeeId, TrainingProgramId = newTrainingProgramId });
            }
            return Json(new { redirectToUrl = Url.Action("Index", "TrainingPrograms"), statusCode = 200, message = "" });
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
                employeeTrainingsJoined.ForEach(e =>
                {
                    employeeTrainingIdsJoinded.Add(e.EmployeeId);
                });
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
            List<string> employeeIdsString = JsonConvert.DeserializeObject<List<string>>(inputTrainingProgram.EmployeeIds);
            List<int> employeeIds = employeeIdsString.Select(int.Parse).ToList();
            _trainingProgramRepository.Update(inputTrainingProgram);


            //Get list old EmployeeTraining
            var oldEmployeeTrainingsJoined = _employeeTrainingRepository.GetByTrainingProgramId(inputTrainingProgram.Id);
            var oldEmployeeTrainingIdsJoinded = new List<int>();
            if (oldEmployeeTrainingsJoined != null)
            {
                oldEmployeeTrainingsJoined.ForEach(e =>
                {
                    oldEmployeeTrainingIdsJoinded.Add(e.EmployeeId);
                });
            }
            var listRemovedEmployeeId = oldEmployeeTrainingIdsJoinded.Except(employeeIds).ToList();
            if (listRemovedEmployeeId != null)
            {
                listRemovedEmployeeId.ForEach(e =>
                {
                    _employeeTrainingRepository.DeleteByTrainingProgramIdAndEmployeeId(inputTrainingProgram.Id, e);
                });
            }

            foreach (var employeeId in employeeIds)
            {
                var existedEmployeeTraining = _employeeTrainingRepository.GetByTrainingProgramIdAndEmployeeId(inputTrainingProgram.Id, employeeId);
                if (existedEmployeeTraining == null)
                {
                    _employeeTrainingRepository.Add(new EmployeeTraining() { EmployeeId = employeeId, TrainingProgramId = inputTrainingProgram.Id });
                }
            }
            return Json(new { redirectToUrl = Url.Action("Index", "TrainingPrograms"), statusCode = 200, message = "" });
        }

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
                employeeTrainingsJoined.ForEach(e =>
                {
                    employeeTrainingIdsJoinded.Add(e.EmployeeId);
                });
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
                            EmployeeId = i.EmployeeId
                        });
                    }
                }
            });
            model.JoinedEmployeeWithDate = listEmployeeWithDate;

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
                employeeTrainingsJoined.ForEach(e =>
                {
                    employeeTrainingIdsJoinded.Add(e.EmployeeId);
                });
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
            List<EmployeeIdWithScore> employeeIdWithScoreString = JsonConvert.DeserializeObject<List<EmployeeIdWithScore>>(input.ListEmployeeIdWithScore);

            // Create new attendance for this training program
            var newAttendanceId = _attendanceRepository.Add(new Attendance() { AttendanceDate = input.AttendanceDate, TrainingProgramId = input.TrainingProgramId });
            if(newAttendanceId != -1)
            {
                //Create new attendance for all joined employees
                employeeIdWithScoreString.ForEach(e =>
                {
                    _attendanceEmployeeRepository.Add(new AttendanceEmployee()
                    {
                        AttendanceId = newAttendanceId,
                        EmployeeId = Int32.Parse(e.EmployeeId),
                        Score = Int32.Parse(e.Score)
                    });
                });
            }

            return Json(new { redirectToUrl = Url.Action("Attendance", "TrainingPrograms", new { id = input.TrainingProgramId }), statusCode = 200, message = "" });
        }
    }
}
