using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Template_HardwareStore.PL.Data;
using Template_HardwareStore.PL.Constants;
using Template_HardwareStore.PL.Models;
using Template_HardwareStore.PL.Models.ViewModels;
using System;
using System.IO;

namespace Template_HardwareStore.PL.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
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
        public IActionResult Upsert(ProductViewModel productViewModel)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;

                string webRootPath = _webHostEnvironment.WebRootPath;

                if (productViewModel.Product.Id == 0)
                {
                    string upload = webRootPath + WebConstants.IamgePath;
                    string fileName = Guid.NewGuid().ToString();
                    string extansion = Path.GetExtension(files[0].FileName);

                    using (var fileStream = new FileStream(Path.Combine(upload, fileName+extansion), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }

                    productViewModel.Product.Image = fileName + extansion;

                    _db.Products.Add(productViewModel.Product);
                    
                }
                else
                {

                }

                _db.SaveChanges();
                _db.Dispose();

                return RedirectToAction("Index");
            }
            return View(productViewModel);
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
