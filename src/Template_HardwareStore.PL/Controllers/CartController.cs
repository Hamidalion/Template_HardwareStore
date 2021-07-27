using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Template_HardwareStore.PL.Data;
using Template_HardwareStore.PL.Extensions;
using Template_HardwareStore.PL.Models;
using Template_HardwareStore.PL.Models.ViewModels;
using Template_HardwareStore.Utility.Constants;

namespace Template_HardwareStore.PL.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IEmailSender _emailSender;

        [BindProperty]
        public ProductUserViewModel ProductUserViewModel { get; set; }


        public CartController(ApplicationDbContext db,
                              IWebHostEnvironment webHostEnvironment,
                              IEmailSender emailSender)
        {
            _db = db;
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

            IEnumerable<Product> prodList = _db.Products.Where(u => prodInCart.Contains(u.Id));

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

            IEnumerable<Product> prodList = _db.Products.Where(u => prodInCart.Contains(u.Id));

            ProductUserViewModel productUserViewModel = new ProductUserViewModel()
            {
                ApplicationUser = _db.ApplicationUsers.FirstOrDefault(u => u.Id == claim.Value),
                ProductList = prodList.ToList()
            };

            double totalPrice = 0;
            foreach (var item in prodList)
            {
                totalPrice += item.Price;
            }
            ViewBag.TotalPrice = totalPrice;

            return View(productUserViewModel);
        }


        [HttpPost, ActionName("Summary")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SummaryPostAsync(ProductUserViewModel ProductUserViewModel)
        {
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

            string messageBody = string.Format(htmlBody, ProductUserViewModel.ApplicationUser.FullName,
                                                         ProductUserViewModel.ApplicationUser.Email,
                                                         ProductUserViewModel.ApplicationUser.PhoneNumber,
                                                         productListSB.ToString());

            await _emailSender.SendEmailAsync(ProductUserViewModel.ApplicationUser.Email, subject, messageBody);

            return RedirectToAction(nameof(InquiryConfirmation));
        }

        public IActionResult InquiryConfirmation()
        {
            HttpContext.Session.Clear();

            return View();
        }

        public IActionResult Remove(int id)
        {
            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();

            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstants.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstants.SessionCart).Count() > 0)
            {
                shoppingCartList = HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstants.SessionCart).ToList();
            }

            shoppingCartList.Remove(shoppingCartList.FirstOrDefault(u => u.ProductId.Equals(id)));

            HttpContext.Session.Set(WebConstants.SessionCart, shoppingCartList);

            return RedirectToAction(nameof(Index));
        }
    }
}
