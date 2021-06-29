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
using Microsoft.AspNetCore.Authorization;

namespace Template_HardwareStore.PL.Controllers
{
    [Authorize(Roles = WebConstants.AdminRole)]
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
            IEnumerable<Product> modelsList = _db.Products.Include(u => u.Category).Include(u => u.ApplicationType);

            //foreach (var item in modelsList)
            //{
            //    item.Category = _db.Categories.FirstOrDefault(c => c.Id == item.CategoryId);
            //    item.ApplicationType = _db.ApplicationTypes.FirstOrDefault(a => a.Id == item.ApplicationTypeId);
            //}

            return View(modelsList);
        }


        // GET - Upsert
        public IActionResult Upsert(int? id)
        {
            #region Example using ViewBag and ViewData
            //IEnumerable<SelectListItem> categoryDropdawn = _db.Categories.Select(c => new SelectListItem
            //{
            //    Text = c.Name,
            //    Value = c.Id.ToString(),
            //});

            ////ViewBag.CategoryDropdawn = categoryDropdawn;
            //ViewData["CategoryDropdawn"] = categoryDropdawn;

            //Product product = new Product();
            #endregion

            ProductViewModel productViewModel = new ProductViewModel()
            {
                Product = new Product(),
                CategorySelectListItem = _db.Categories.Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString(),
                }),

                ApplicationTypeSelectListItem = _db.ApplicationTypes.Select(a => new SelectListItem
                {
                    Text = a.Name,
                    Value = a.Id.ToString(),
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
                string upload = webRootPath + WebConstants.IamgePath;
                string fileName = Guid.NewGuid().ToString();
                string extansion = Path.GetExtension(files[0].FileName);

                if (productViewModel.Product.Id == 0)
                {
                    // Creat Product

                    using (var fileStream = new FileStream(Path.Combine(upload, fileName + extansion), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }

                    productViewModel.Product.Image = fileName + extansion;

                    _db.Products.Add(productViewModel.Product);
                }
                else
                {
                    // Edit Product

                    var productModel = _db.Products.AsNoTracking().FirstOrDefault(p => p.Id == productViewModel.Product.Id);

                    if (files.Count > 0)
                    {
                        var oldFile = Path.Combine(upload, productModel.Image);
                        if (System.IO.File.Exists(oldFile))
                        {
                            System.IO.File.Delete(oldFile);
                        }

                        using (var fileStream = new FileStream(Path.Combine(upload, fileName + extansion), FileMode.Create))
                        {
                            files[0].CopyTo(fileStream);
                        }

                        productViewModel.Product.Image = fileName + extansion;
                    }
                    else
                    {
                        productViewModel.Product.Image = productModel.Image;
                    }

                    _db.Products.Update(productViewModel.Product);
                }

                _db.SaveChanges();
                _db.Dispose();

                return RedirectToAction("Index");
            }

            productViewModel.CategorySelectListItem = _db.Categories.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString(),
            });

            productViewModel.ApplicationTypeSelectListItem = _db.ApplicationTypes.Select(a => new SelectListItem
            {
                Text = a.Name,
                Value = a.Id.ToString(),
            });

            return View(productViewModel);
        }

        // GET - Delete
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var productModel = _db.Products.Include(c => c.Category)
                                           .Include(c => c.ApplicationType)
                                           .FirstOrDefault(c => c.Id == id); //egger loading

            //productModel.Category = _db.Categories.Find(productModel.CategoryId);

            if (productModel == null)
            {
                return NotFound();
            }

            return View(productModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken] //токен защиты от взлома
        [ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            var model = _db.Products.Find(id);
            if (ModelState.IsValid && model != null)
            {
                string upload = _webHostEnvironment.WebRootPath + WebConstants.IamgePath; ;
                string fileName = model.Image;

                var file = Path.Combine(upload, fileName);
                if (System.IO.File.Exists(file))
                {
                    System.IO.File.Delete(file);
                }

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
