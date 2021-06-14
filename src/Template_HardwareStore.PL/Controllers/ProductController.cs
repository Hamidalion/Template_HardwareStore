using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Template_HardwareStore.PL.Data;
using Template_HardwareStore.PL.Models;
using Template_HardwareStore.PL.Models.ViewModels;

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


        // GET - Upsert
        public IActionResult Upsert(int? id)
        {
            //IEnumerable<SelectListItem> categoryDropdawn = _db.Categories.Select(c => new SelectListItem
            //{
            //    Text = c.Name,
            //    Value = c.Id.ToString(),
            //});

            ////ViewBag.CategoryDropdawn = categoryDropdawn;
            //ViewData["CategoryDropdawn"] = categoryDropdawn;

            //Product product = new Product();

            ProductViewModel productViewModel = new ProductViewModel() {
                Product = new Product(),
                CategorySelectListItem = _db.Categories.Select(c => new SelectListItem
                    {
                        Text = c.Name,
                        Value = c.Id.ToString(),
                    })
            };   

            if (id == null || id == 0)
            {
                return View(productViewModel);
            }
            else
            {
                productViewModel.Product = _db.Products.Find(id);
                if (productViewModel.Product == null)
                {
                    return NotFound();
                }
                else
                {
                    return View(productViewModel);
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken] //токен защиты от взлома
        public IActionResult Upsert(Product product)
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
        public IActionResult DeletePost(int? id)
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
