using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Template_HardwareStore.DAL.Repository;
using Template_HardwareStore.DAL.Repository.Interface;
using Template_HardwareStore.Entities.Models;
using Template_HardwareStore.Entities.Models.ViewModels;
using Template_HardwareStore.Utility.Constants;
using Template_HardwareStore.Utility.Extensions;

namespace Template_HardwareStore.PL.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly IApplicationUserRepository _applicationUserRepository;
        private readonly IInquiryDetailRepository _inquiryDetailRepository;
        private readonly IInquiryHeaderRepository _inquiryHeaderRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IEmailSender _emailSender;

        [BindProperty]
        public ProductUserViewModel ProductUserViewModel { get; set; }


        public CartController(IProductRepository productRepository,
                              IApplicationUserRepository applicationUserRepository,
                              IInquiryDetailRepository inquiryDetailRepository,
                              IInquiryHeaderRepository inquiryHeaderRepository,
                              IWebHostEnvironment webHostEnvironment,
                              IEmailSender emailSender)
        {
            _productRepository = productRepository;
            _applicationUserRepository = applicationUserRepository;
            _inquiryDetailRepository = inquiryDetailRepository;
            _inquiryHeaderRepository = inquiryHeaderRepository;
            _webHostEnvironment = webHostEnvironment;
            _emailSender = emailSender;
        }

        public IActionResult Index()
        {
            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();

            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstants.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstants.SessionCart).Count() > 0)
            {
                shoppingCartList = HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstants.SessionCart).ToList();
            }

            List<int> prodInCart = shoppingCartList.Select(u => u.ProductId).ToList();

            IEnumerable<Product> prodList = _productRepository.GetAll(u => prodInCart.Contains(u.Id));

            return View(prodList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Index")]
        public IActionResult IndexPost()
        {
            return RedirectToAction(nameof(Summary));
        }

        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier); // Если объект вошел в систему он будет определен
                                                                             //var userId = await _accountManager.GetUserIdByNameAsync(User.Identity.Name);  Еще Способ
                                                                             //var userId = User.FindFirstValue(ClaimTypes.Name); Еще способ

            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();

            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstants.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstants.SessionCart).Count() > 0)
            {
                shoppingCartList = HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstants.SessionCart).ToList();
            }

            List<int> prodInCart = shoppingCartList.Select(u => u.ProductId).ToList();

            IEnumerable<Product> prodList = _productRepository.GetAll(u => prodInCart.Contains(u.Id));

            ProductUserViewModel productUserViewModel = new ProductUserViewModel()
            {
                ApplicationUser = _applicationUserRepository.FirstOrDefault(u => u.Id == claim.Value),
                ProductList = prodList.ToList()
            };

            double totalPrice = 0;
            foreach (var item in prodList)
            {
                totalPrice += item.Price;
            }
            ViewBag.TotalPrice = totalPrice;

            if (productUserViewModel != null)
            {
                TempData[WebConstants.Success] = "Product loaded seccessfully.";
            }
            else
            {
                TempData[WebConstants.Error] = "Not Product for load!";
            }

            return View(productUserViewModel);
        }

        [HttpPost, ActionName("Summary")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SummaryPostAsync(ProductUserViewModel productUserViewModel)
        {
            try
            {
                var claimIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);

                var pathToTemplate = _webHostEnvironment.WebRootPath + Path.DirectorySeparatorChar.ToString()
                                                       + "templates" + Path.DirectorySeparatorChar.ToString()
                                                       + "Inquiry.html";

                var subject = "New Inquary";
                string htmlBody = "";

                using (StreamReader sr = System.IO.File.OpenText(pathToTemplate))
                {
                    htmlBody = sr.ReadToEnd();
                }

                StringBuilder productListSB = new StringBuilder();

                foreach (var item in ProductUserViewModel.ProductList)
                {
                    productListSB.Append($" - Name: {item.Name} <span style='font-size: 14px' > (ID: {item.Id}) </span> <br /> ");
                }

                string messageBody = string.Format(htmlBody, productUserViewModel.ApplicationUser.FullName,
                                                             productUserViewModel.ApplicationUser.Email,
                                                             productUserViewModel.ApplicationUser.PhoneNumber,
                                                             productListSB.ToString());

                await _emailSender.SendEmailAsync(ProductUserViewModel.ApplicationUser.Email, subject, messageBody);

                InquiryHeader inquiryHeader = new InquiryHeader()
                {
                    ApplicationUserId = claim.Value,
                    FullName = productUserViewModel.ApplicationUser.FullName,
                    Email = productUserViewModel.ApplicationUser.Email,
                    PhoneNamber = productUserViewModel.ApplicationUser.PhoneNumber,
                    InquiryDate = DateTime.Now
                };

                _inquiryHeaderRepository.Add(inquiryHeader);
                _inquiryHeaderRepository.Save();

                foreach (var product in productUserViewModel.ProductList)
                {
                    InquiryDetail inquiryDetail = new InquiryDetail()
                    {
                        InquiryHeaderId = inquiryHeader.Id,
                        ProductId = product.Id,
                    };

                    _inquiryDetailRepository.Add(inquiryDetail);
                }

                _inquiryDetailRepository.Save();

                TempData[WebConstants.Success] = "Order added seccessfully.";
            }
            catch (Exception ex)
            {
                TempData[WebConstants.Error] = $"Order complete with error {ex}!";
            }

            return RedirectToAction(nameof(InquiryConfirmation));
        }

        public IActionResult InquiryConfirmation()
        {
            HttpContext.Session.Clear();

            return View();
        }

        public IActionResult Remove(int id)
        {
            try
            {
                List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();

                if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstants.SessionCart) != null
                    && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstants.SessionCart).Count() > 0)
                {
                    shoppingCartList = HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstants.SessionCart).ToList();
                }

                shoppingCartList.Remove(shoppingCartList.FirstOrDefault(u => u.ProductId.Equals(id)));

                HttpContext.Session.Set(WebConstants.SessionCart, shoppingCartList);

                TempData[WebConstants.Success] = "Order deliteded seccessfully.";
            }
            catch (Exception ex)
            {
                TempData[WebConstants.Error] = $"Order deleted with error {ex}!";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
