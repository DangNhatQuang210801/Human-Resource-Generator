using AutoMapper;
using Human_Resource_Generator.Interfaces;
using Human_Resource_Generator.Models;
using Human_Resource_Generator.ViewModels.EmployeeViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Human_Resource_Generator.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;

        public EmployeeController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // GET: EmployeeController
        public ActionResult Index()
        {
            var model = _unitOfWork.Employee.GetAll();
            var vm = _mapper.Map<List<EmployeeViewModel>>(model);
            return View(vm);
        }

        // GET: EmployeeController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: EmployeeController/Create
        public ActionResult Create()
        {
            var TrainingProRepo = _unitOfWork.TrainingProgram.GetAll();
            var selectList = new List<SelectListItem>();
            foreach (var item in TrainingProRepo)
            {
                selectList.Add(new SelectListItem(item.program_name,item.program_id));
            }
            var vm = new CreateEmployeeViewModel()
            {
                TrainingPrograms = selectList
            };
            return View(vm);
        }

        // POST: EmployeeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateEmployeeViewModel vm)
        {
            try
            {
                Employee employee = new Employee()
                {
                    Number = vm.Number,
                    Name = vm.Name,
                    Department = vm.Department,
                    Birthday = vm.Birthday
                };
                foreach ( var item in vm.SelectedTrainingProgram)
                {
                    employee.EmployeeTrainings.Add(new EmployeeTraining(){
                        program_id = item
                    });
                }
                _unitOfWork.Employee.Insert(employee);
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: EmployeeController/Edit/5
        public ActionResult Edit(string id)
        {
            var employees = _unitOfWork.Employee.GetById(id);
            var trainingprograms = _unitOfWork.TrainingProgram.GetAll();
            var selectTrainingPrograms = employees.EmployeeTrainings.Select(x => new TrainingProgram()
            {
                program_id = x.TrainingProgram.program_id,
                program_name = x.TrainingProgram.program_name,
            });
            var selectList = new List<SelectListItem>();
            trainingprograms.ForEach(item => selectList.Add(new SelectListItem(item.program_name, item.program_id, selectTrainingPrograms.Select(x => x.program_id).Contains(item.program_id))));
            var vm = new EditEmployeeViewModel()
            {
                ID = employees.ID,
                Number = employees.Number,
                Name   = employees.Name,
                Department = employees.Department,
                Birthday = employees.Birthday,
                TrainingPrograms = selectList
            };
            return View(vm);
        }

        // POST: EmployeeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditEmployeeViewModel vm)
        {
            try
            {
                // var employee = _unitOfWork.Employee.GetById(vm.ID);
                //employee.Name = vm.Name;
                //employee.Department = vm.Department;
                //employee.Birthday = vm.Birthday;
                //var selectedTrainingProgram = vm.SelectedTrainingProgram;
                //var existingTrainingProgram = employee.EmployeeTrainings.Select(x => x.program_id).ToList();
                //var toAdd = selectedTrainingProgram.Except(existingTrainingProgram).ToList();
                //employee.EmployeeTrainings = employee.EmployeeTrainings.Where(x =>!ToRemove.Contains(x.program_id)).ToList();
                //foreach(var item in toAdd) {
                //    employee.EmployeeTrainings.Add(new EmployeeTraining()
                //    {
                //        program_id = item,
                //        ID = employee.ID,
                //    });
                //}
                //_unitOfWork.Save();
                return RedirectToAction("Index", "Employee");
            }
            catch
            {
                return View();
            }
        }

        // GET: EmployeeController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: EmployeeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
