using System.IdentityModel.Tokens.Jwt;
using AutoMapper;
using Human_Resource_Generator.Interfaces;
using Human_Resource_Generator.Models;
using Human_Resource_Generator.Repository;
using Human_Resource_Generator.ViewModels.EmployeeViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace Human_Resource_Generator.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepo _employeeRepo;

        public EmployeeController(IEmployeeRepo e)
        {
            _employeeRepo = e;
        }

        // GET: EmployeeController
        public ActionResult Index()
        {
            var model = _employeeRepo.GetAll().ToList();
            return View(model);
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
            return RedirectToAction("Index");
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
            return Ok(employee);
        }

        [HttpPost]
        public ActionResult EditEmployee(string a)
        {
            return Ok();
        }
        
        
    }
}
