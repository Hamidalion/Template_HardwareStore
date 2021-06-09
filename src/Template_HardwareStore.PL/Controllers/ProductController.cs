using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Template_HardwareStore.PL.Data;
using Template_HardwareStore.PL.Models;

namespace Template_HardwareStore.PL.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ProductController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> modelsList = _db.Products;

            foreach (var item in modelsList)
            {
                item.Category = _db.Categories.FirstOrDefault(i => i.Id == item.CategoryId);
            }

            return View(modelsList);
        }

        // GET - Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken] //токен защиты от взлома
        public IActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                _db.Products.Add(product);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        // GET - Edit
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var model = _db.Products.Find(id);
            if (model == null)
            {
                return NotFound();
            }
            
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken] //токен защиты от взлома
        public IActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                _db.Products.Update(product);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        // GET - Delete
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var model = _db.Products.Find(id);
            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken] //токен защиты от взлома
        [ActionName("Delete")]
        public IActionResult DeletePost (int? id)
        {
            var model = _db.Products.Find(id);
            if (ModelState.IsValid && model != null)
            {
                _db.Products.Remove(model);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return NotFound();
            }
        }

    }
}
