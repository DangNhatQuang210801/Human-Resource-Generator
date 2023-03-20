
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Human_Resource_Generator.Data;
using Human_Resource_Generator.Models;

namespace Human_Resource_Generator.Controllers
{
    public class EmployeeTrainingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmployeeTrainingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: EmployeeTrainings
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.EmployeeTrainings.Include(e => e.Employee).Include(e => e.TrainingProgram);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: EmployeeTrainings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.EmployeeTrainings == null)
            {
                return NotFound();
            }

            var employeeTraining = await _context.EmployeeTrainings
                .Include(e => e.Employee)
                .Include(e => e.TrainingProgram)
                .FirstOrDefaultAsync(m => m.employee_id == id);
            if (employeeTraining == null)
            {
                return NotFound();
            }

            return View(employeeTraining);
        }

        // GET: EmployeeTrainings/Create
        public IActionResult Create()
        {
            ViewData["program_id"] = new SelectList(_context.Employees, "employee_id", "employee_id");
            ViewData["program_id"] = new SelectList(_context.TrainingPrograms, "program_id", "program_id");
            return View();
        }

        // POST: EmployeeTrainings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("employee_id,program_id")] EmployeeTraining employeeTraining)
        {
            if (ModelState.IsValid)
            {
                _context.Add(employeeTraining);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["program_id"] = new SelectList(_context.Employees, "employee_id", "employee_id", employeeTraining.program_id);
            ViewData["program_id"] = new SelectList(_context.TrainingPrograms, "program_id", "program_id", employeeTraining.program_id);
            return View(employeeTraining);
        }

        // GET: EmployeeTrainings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.EmployeeTrainings == null)
            {
                return NotFound();
            }

            var employeeTraining = await _context.EmployeeTrainings.FindAsync(id);
            if (employeeTraining == null)
            {
                return NotFound();
            }
            ViewData["program_id"] = new SelectList(_context.Employees, "employee_id", "employee_id", employeeTraining.program_id);
            ViewData["program_id"] = new SelectList(_context.TrainingPrograms, "program_id", "program_id", employeeTraining.program_id);
            return View(employeeTraining);
        }

        // POST: EmployeeTrainings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("employee_id,program_id")] EmployeeTraining employeeTraining)
        {
            if (id != employeeTraining.employee_id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employeeTraining);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeTrainingExists(employeeTraining.employee_id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["program_id"] = new SelectList(_context.Employees, "employee_id", "employee_id", employeeTraining.program_id);
            ViewData["program_id"] = new SelectList(_context.TrainingPrograms, "program_id", "program_id", employeeTraining.program_id);
            return View(employeeTraining);
        }

        // GET: EmployeeTrainings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.EmployeeTrainings == null)
            {
                return NotFound();
            }

            var employeeTraining = await _context.EmployeeTrainings
                .Include(e => e.Employee)
                .Include(e => e.TrainingProgram)
                .FirstOrDefaultAsync(m => m.employee_id == id);
            if (employeeTraining == null)
            {
                return NotFound();
            }

            return View(employeeTraining);
        }

        // POST: EmployeeTrainings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.EmployeeTrainings == null)
            {
                return Problem("Entity set 'ApplicationDbContext.EmployeeTrainings'  is null.");
            }
            var employeeTraining = await _context.EmployeeTrainings.FindAsync(id);
            if (employeeTraining != null)
            {
                _context.EmployeeTrainings.Remove(employeeTraining);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeTrainingExists(int id)
        {
          return (_context.EmployeeTrainings?.Any(e => e.employee_id == id)).GetValueOrDefault();
        }
    }
}
