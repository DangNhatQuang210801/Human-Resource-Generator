using Human_Resource_Generator.Models;
using Human_Resource_Generator.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Human_Resource_Generator.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeTrainingRepository _employeeRepo;

        public EmployeeController(IEmployeeTrainingRepository employeeRepo)
        {
            _employeeRepo = employeeRepo;
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
            return View(employee);
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
