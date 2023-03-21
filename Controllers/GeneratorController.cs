using Human_Resource_Generator.Data;
using Human_Resource_Generator.Models;
using Human_Resource_Generator.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Human_Resource_Generator.Controllers
{
    public class GeneratorController : Controller
    {
        private readonly IGeneratorRepo _generatorRepo;
        private ApplicationDbContext _db;

        public GeneratorController(IGeneratorRepo generatorRepo)
        {
            _generatorRepo = generatorRepo;
        }

        public IActionResult Index()
        {
            var data = _generatorRepo.GetAllEmployeesJoinedAnyTrainingProgram();
            return View(data);
        }

        //Get
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(EmployeeTraining obj) {
            object value = _generatorRepo.EmployeeTraining.Add(obj);
            object value1 = _generatorRepo.SaveChanges();
        return RedirectToAction("Index");   
        }

    }
}
