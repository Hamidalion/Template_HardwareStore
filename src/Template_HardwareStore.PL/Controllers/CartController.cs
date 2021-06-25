using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Template_HardwareStore.PL.Constants;
using Template_HardwareStore.PL.Data;
using Template_HardwareStore.PL.Extensions;
using Template_HardwareStore.PL.Models;
using Template_HardwareStore.PL.Models.ViewModels;

namespace Template_HardwareStore.PL.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _db;
        
        [BindProperty]
        public ProductUserViewModel ProductUserViewModel { get; set; }

        public CartController(ApplicationDbContext db)
        {
            _db = db;
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
                ProductList = prodList
            };

            double totalPrice = 0;
            foreach (var item in prodList)
            {
                totalPrice += item.Price;
            }
            ViewBag.TotalPrice = totalPrice;

            return View(productUserViewModel);
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
