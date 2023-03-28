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
        public async Task<IActionResult> Index(string SearchName)
        {
            ViewData["CurrentFilter"] = SearchName;
            if (String.IsNullOrEmpty(SearchName))
            {
                var data = _generatorRepo.GetAllEmployeesJoinedAnyTrainingProgram();
                return View(data);
            }
            else
            {
                var searchData = _generatorRepo.SearchAllEmployee(SearchName);
                return View(searchData);
            }
        }

        public IActionResult Create(Employee employee)
        {
            var result = _generatorRepo.CreateEmployee(employee);
            if (result)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

    }
}
