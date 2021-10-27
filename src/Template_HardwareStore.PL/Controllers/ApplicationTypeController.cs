using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Template_HardwareStore.DAL.Repository.Interface;
using Template_HardwareStore.Entities.Models;
using Template_HardwareStore.Utility.Constants;

namespace Template_HardwareStore.PL.Controllers
{
    [Authorize(Roles = WebConstants.AdminRole)]
    public class ApplicationTypeController : Controller
    {
        private readonly IApplicationTypeRepository _repository;

        public ApplicationTypeController(IApplicationTypeRepository repository)
        {
            _repository = repository;
        }

        public IActionResult Index()
        {
            IEnumerable<ApplicationType> modelsList = _repository.GetAll();
            return View(modelsList);
        }

        // GET - Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken] //токен защиты от взлома
        public IActionResult Create(ApplicationType applicationType)
        {
            if (ModelState.IsValid)
            {
                _repository.Add(applicationType);
                _repository.Save();

                TempData[WebConstants.Success] = "Application Type added seccessfully.";

                return RedirectToAction("Index");
            }
            else
            {
                TempData[WebConstants.Error] = "Error of creating Application Type!";
            }
            return View(applicationType);
        }

        // GET - Edit
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var model = _repository.FindById(id.GetValueOrDefault());
            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken] //токен защиты от взлома
        public IActionResult Edit(ApplicationType applicationType)
        {
            if (ModelState.IsValid)
            {
                _repository.Update(applicationType);
                _repository.Save();

                TempData[WebConstants.Success] = "Application Type edited seccessfully.";

                return RedirectToAction("Index");
            }
            else
            {
                TempData[WebConstants.Error] = "Application Type edited with error!";
            }
            return View(applicationType);
        }

        // GET - Delete
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var model = _repository.FindById(id.GetValueOrDefault());
            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken] //токен защиты от взлома
        [ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            var model = _repository.FindById(id.GetValueOrDefault());
            if (ModelState.IsValid && model != null)
            {
                _repository.Remove(model);
                _repository.Save();

                TempData[WebConstants.Success] = "Application Type deliteded seccessfully.";

                return RedirectToAction("Index");
            }
            else
            {
                TempData[WebConstants.Error] = "Application Type deleted with error!";

                return NotFound();
            }
        }
    }
}
