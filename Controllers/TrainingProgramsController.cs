using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Human_Resource_Generator.Data;
using Human_Resource_Generator.Models;
using Human_Resource_Generator.Interfaces;
using Human_Resource_Generator.Repository;

namespace Human_Resource_Generator.Controllers
{
    public class TrainingProgramsController : Controller
    {
        private readonly ITrainingProgramRepository _trainingProgramRepository;

        public TrainingProgramsController(ITrainingProgramRepository trainingProgramRepository)
        {
            _trainingProgramRepository = trainingProgramRepository;
        }

        // GET: TrainingPrograms
        public IActionResult Index()
        {
            var model = _trainingProgramRepository.GetAll().ToList();
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

            return View(trainingProgram);
        }

        // GET: TrainingPrograms/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TrainingPrograms/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Name,Description,Teacher")] TrainingProgram trainingProgram)
        {
            trainingProgram.CreatedAt = DateTime.Now;
            trainingProgram.UpdatedAt = DateTime.Now;
            _trainingProgramRepository.Add(trainingProgram);
            return RedirectToAction(nameof(Index));
        }

        // GET: TrainingPrograms/Edit/5
        public IActionResult Edit(int id)
        {
            var trainingProgram = _trainingProgramRepository.GetById(id);
            if (trainingProgram == null)
            {
                return NotFound();
            }
            return View(trainingProgram);
        }

        // POST: TrainingPrograms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Name,Description,Teacher,CreatedAt")] TrainingProgram trainingProgram)
        {
            if (id != trainingProgram.Id)
            {
                return NotFound();
            }
            trainingProgram.UpdatedAt = DateTime.Now;
            _trainingProgramRepository.Update(trainingProgram);
            return RedirectToAction(nameof(Index));
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
                _trainingProgramRepository.Delete(trainingProgram);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
