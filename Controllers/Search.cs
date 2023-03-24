using Human_Resource_Generator.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Human_Resource_Generator.Controllers
{
    public class Search : Controller
    {
        private readonly IGeneratorRepo _generatorRepo;
        public GeneratorController(IGeneratorRepo generatorRepo)
        {
            _generatorRepo = generatorRepo;
        }
        public async Task<IActionResult> Search(string SearchString)
        {
            ViewData["CurrentFilter"] = SearchString;
            var employee = from t in _generatorRepo.GetAllEmployeesJoinedAnyTrainingProgram()
                           select t;
            if (!String.IsNullOrEmpty(SearchString))
            {
                employee = employee.Where(t => t.Employee.employee_name.Contains(SearchString));
            }
            return View(employee);
        }
    }
}
