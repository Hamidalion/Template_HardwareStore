using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using Template_HardwareStore.DAL.Repository.Interface;
using Template_HardwareStore.Entities.Models;
using Template_HardwareStore.Entities.Models.ViewModels;
using Template_HardwareStore.Utility.Constants;

namespace Template_HardwareStore.PL.Controllers
{
    [Authorize(Roles = WebConstants.AdminRole)]
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(IProductRepository productRepository, IWebHostEnvironment webHostEnvironment)
        {
            _productRepository = productRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> modelsList = _productRepository.GetAll(includeProperties: $"{WebConstants.CategoryName},{WebConstants.ApplicationTypeName}");

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
                CategorySelectListItem = _productRepository.GetAllDropDawnList(WebConstants.CategoryName),
                ApplicationTypeSelectListItem = _productRepository.GetAllDropDawnList(WebConstants.ApplicationTypeName)
            };

            if (id == null || id == 0)
            {
                return View(productViewModel);
            }
            else
            {
                productViewModel.Product = _productRepository.FindById(id.GetValueOrDefault());
                if (productViewModel.Product == null)
                {
                    TempData[WebConstants.Error] = "Product not find!";

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
            try
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

                        // Creat Product
                        using (var fileStream = new FileStream(Path.Combine(upload, fileName + extansion), FileMode.Create))
                        {
                            files[0].CopyTo(fileStream);
                        }

                        productViewModel.Product.Image = fileName + extansion;

                        _productRepository.Add(productViewModel.Product);
                    }
                    else
                    {
                        // Edit Product
                        var productModel = _productRepository.FirstOrDefault(p => p.Id == productViewModel.Product.Id, isTracking: false);

                        if (files.Count > 0)
                        {
                            string upload = webRootPath + WebConstants.IamgePath;
                            string fileName = Guid.NewGuid().ToString();
                            string extansion = Path.GetExtension(files[0].FileName);

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

                        _productRepository.Update(productViewModel.Product);
                    }

                    _productRepository.Save();

                    return RedirectToAction("Index");
                }

                TempData[WebConstants.Success] = "Product edited seccessfully.";
            }
            catch (Exception ex)
            {
                TempData[WebConstants.Error] = $"Product edited with error {ex}!";
            }

            productViewModel.CategorySelectListItem = _productRepository.GetAllDropDawnList(WebConstants.CategoryName);
            productViewModel.ApplicationTypeSelectListItem = _productRepository.GetAllDropDawnList(WebConstants.ApplicationTypeName);

            return View(productViewModel);
        }

        // GET - Delete
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var productModel = _productRepository.FirstOrDefault(c => c.Id == id, includeProperties: $"{WebConstants.CategoryName},{WebConstants.ApplicationTypeName}");

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
            var model = _productRepository.FindById(id.GetValueOrDefault());
            if (ModelState.IsValid && model != null)
            {
                string upload = _webHostEnvironment.WebRootPath + WebConstants.IamgePath; ;
                string fileName = model.Image;

                var file = Path.Combine(upload, fileName);
                if (System.IO.File.Exists(file))
                {
                    System.IO.File.Delete(file);
                }

                _productRepository.Remove(model);
                _productRepository.Save();

                TempData[WebConstants.Success] = "Product removed seccessfully.";

                return RedirectToAction("Index");
            }
            else
            {
                return NotFound();
            }
        }
    }
}
