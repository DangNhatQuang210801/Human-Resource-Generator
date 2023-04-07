using Microsoft.AspNetCore.Mvc;
using Human_Resource_Generator.Models;
using Human_Resource_Generator.Repository;
using Human_Resource_Generator.ViewModels.TrainingProgramViewModel;
using Newtonsoft.Json;
using AutoMapper;

namespace Human_Resource_Generator.Controllers
{
    public class TrainingProgramsController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ITrainingProgramRepository _trainingProgramRepository;
        private readonly IEmployeeTrainingRepository _employeeTrainingRepository;

        public TrainingProgramsController(IMapper mapper, ITrainingProgramRepository trainingProgramRepository, IEmployeeTrainingRepository employeeTrainingRepository)
        {
            _trainingProgramRepository = trainingProgramRepository;
            _employeeTrainingRepository = employeeTrainingRepository;
            _mapper = mapper;
        }

        // GET: TrainingPrograms
        public IActionResult Index()
        {
            var model = _trainingProgramRepository.GetAll();
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
    }
}
