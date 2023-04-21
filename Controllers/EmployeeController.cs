using Human_Resource_Generator.Models;
using Human_Resource_Generator.Repository;
using Human_Resource_Generator.ViewModels.EmployeeViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Human_Resource_Generator.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeTrainingRepository _employeeRepo;
        private readonly ITrainingProgramRepository _trainingProgramRepository;

        public EmployeeController(IEmployeeTrainingRepository employeeRepo, ITrainingProgramRepository trainingProgramRepository)
        {
            _employeeRepo = employeeRepo;
            _trainingProgramRepository = trainingProgramRepository;
        }

        // GET: EmployeeController
        public ActionResult Index(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                var model = _employeeRepo.GetAll();
                return View(model);
            }
            else
            {
                var data = _employeeRepo.GetEmployeesByName(name);
                return View(data);
            }
        }

        // GET: EmployeeController/Details/5
        public ActionResult Details(int id)
        {
            var employee = _employeeRepo.GetById(id);
            var employeeDetailViewModel = new EmployeeDetailViewModel
            {
                Id = employee.Id,
                Code = employee.Code,
                Name = employee.Name,
                DateOfBirth = employee.DateOfBirth,
                Position = employee.Position,
                AttendanceEmployees = employee.AttendanceEmployees,
                EmployeeTrainings = employee.EmployeeTrainings,
                Score = new List<KeyValuePair<int, int>>()
            };
            foreach (var employeeTraining in employee.EmployeeTrainings)
            {
                var score = _trainingProgramRepository.GetScoreEmployee(employeeTraining.TrainingProgramId,employee.Id);
                employeeDetailViewModel.Score.Add(new KeyValuePair<int, int>(employeeTraining.TrainingProgramId,score));
            }
            return View(employeeDetailViewModel);
        }
        
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult Create(Employee employee)
        {
            _employeeRepo.Insert(employee);
            return RedirectToAction("Index");
        }
        
        public ActionResult Delete(int id)
        {
            _employeeRepo.Delete(id);
            return RedirectToAction("Index",false);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var employee = _employeeRepo.GetById(id);
            return View(employee);
        }
        
        [HttpPost]
        public IActionResult Edit(Employee employee)
        {
            _employeeRepo.Update(employee);
            return RedirectToAction("Index");
        }
    }
}
