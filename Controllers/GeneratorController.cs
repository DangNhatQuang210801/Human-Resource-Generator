using Human_Resource_Generator.Data;
using Microsoft.AspNetCore.Mvc;

namespace Human_Resource_Generator.Controllers
{
    public class GeneratorController : Controller
    {
        private readonly ApplicationDbContext _db;
        public GeneratorController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            return View();
        }
    }
}
