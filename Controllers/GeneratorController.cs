using Human_Resource_Generator.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Human_Resource_Generator.Controllers
{
    public class GeneratorController : Controller
    {
        private readonly IGeneratorRepo _generatorRepo;

        public GeneratorController(IGeneratorRepo generatorRepo)
        {
            _generatorRepo = generatorRepo;
        }

        public IActionResult Index()
        {
            var data = _generatorRepo.GetAllEmployeesJoinedAnyTrainingProgram();
            return View(data);
        }
    }
}
