using AutoMapper;
using Human_Resource_Generator.Interfaces;
using Human_Resource_Generator.Models;
using Human_Resource_Generator.ViewModels.TrainingProgramViewModel;
using Human_Resource_Generator.ViewModels.TrainingProgramViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Human_Resource_Generator.Controllers
{
    public class TrainingProgramController : Controller
    {
        // GET: TrainingProgramController
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;

        public TrainingProgramController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public ActionResult Index()
        {
            var model = _unitOfWork.TrainingProgram.GetAll();
            var vm = _mapper.Map<List<TrainingProgramViewModel>>(model);
            return View(vm);
        }

        // GET: TrainingProgramController/Details/5
        public ActionResult Details(int id)
        {
            var model = _unitOfWork.TrainingProgram.GetById (id);
            var vm = _mapper.Map<TrainingProgramViewModel>(model);
            return View(vm);
        }

        // GET: TrainingProgramController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TrainingProgramController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateTrainingProgramViewModel vm)
        {
            try
            {
                var model = _mapper.Map<TrainingProgram>(vm);
                _unitOfWork.TrainingProgram.Insert(model);
                _unitOfWork.Save();
                return RedirectToAction("Index", "TrainingProgram");
            }
            catch
            {
                return View();
            }
        }

        // GET: TrainingProgramController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: TrainingProgramController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
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

        // GET: TrainingProgramController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: TrainingProgramController/Delete/5
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
