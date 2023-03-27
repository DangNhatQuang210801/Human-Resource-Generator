using Human_Resource_Generator.Data;
using Human_Resource_Generator.Models;
using Human_Resource_Generator.Repository;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Core.Types;

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

        //Get
        public IActionResult Create()
        {
            return View();
        }
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Create(EmployeeTraining obj) {
        //    object value = _generatorRepo.EmployeeTraining.Add(obj);
        //    object value1 = _generatorRepo.SaveChanges();
        //return RedirectToAction("Index");   
        //}

    }
}
