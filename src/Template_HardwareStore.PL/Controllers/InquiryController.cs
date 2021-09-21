using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Template_HardwareStore.DAL.Repository.Interface;
using Template_HardwareStore.Entities.Models;
using Template_HardwareStore.Entities.Models.ViewModels;
using Template_HardwareStore.Utility.Constants;
using Template_HardwareStore.Utility.Extensions;

namespace Template_HardwareStore.PL.Controllers
{
    public class InquiryController : Controller
    {
        private readonly IInquiryHeaderRepository _inquiryHeaderRepository;
        private readonly IInquiryDetailRepository _inquiryDetailRepository;

        [BindProperty]
        public InquiryViewModel InquiryViewModel { get; set; }


        public InquiryController(IInquiryHeaderRepository inquiryHeaderRepository, IInquiryDetailRepository inquiryDetailRepository)
        {
            _inquiryHeaderRepository = inquiryHeaderRepository;
            _inquiryDetailRepository = inquiryDetailRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details(int id)
        {
            InquiryViewModel = new InquiryViewModel()
            {
                InquiryHeader = _inquiryHeaderRepository.FirstOrDefault(u => u.Id == id),
                InquiryDetail = _inquiryDetailRepository.GetAll(u => u.InquiryHeaderId == id, includeProperties: "Product")
            };
            return View(InquiryViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // Фильтр ValidateAntiforgeryToken предназначен для противодействия подделке межсайтовых запросов, производя верификацию токенов при обращении к методу действия. Наиболее частым случаем является применение данного фильтра к методам, отвечающим за авторизацию
        public IActionResult Details()
        {
            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();

            InquiryViewModel.InquiryDetail = _inquiryDetailRepository.GetAll(u => u.InquiryHeaderId == InquiryViewModel.InquiryHeader.Id);

            foreach (var detail in InquiryViewModel.InquiryDetail)
            {
                ShoppingCart shoppingCart = new ShoppingCart()
                {
                    ProductId = detail.ProductId
                };

                shoppingCartList.Add(shoppingCart);
            }

            HttpContext.Session.Clear();
            HttpContext.Session.Set(WebConstants.SessionCart, shoppingCartList);
            HttpContext.Session.Set(WebConstants.SessionInquaryId, InquiryViewModel.InquiryHeader.Id);

            return RedirectToAction("Index", "Cart");
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // Фильтр ValidateAntiforgeryToken предназначен для противодействия подделке межсайтовых запросов, производя верификацию токенов при обращении к методу действия. Наиболее частым случаем является применение данного фильтра к методам, отвечающим за авторизацию
        public IActionResult Delete(string id)
        {
            InquiryHeader inquiryHeader = _inquiryHeaderRepository.FirstOrDefault(u => u.Id == InquiryViewModel.InquiryHeader.Id);
            IEnumerable<InquiryDetail> inquiryDetails = _inquiryDetailRepository.GetAll(u => u.InquiryHeaderId == InquiryViewModel.InquiryHeader.Id);

            _inquiryDetailRepository.RemoveRange(inquiryDetails);
            _inquiryHeaderRepository.Remove(inquiryHeader);
            _inquiryHeaderRepository.Save();  // не надо здесь вызывать два сохраннения, достаточно только одного

            return RedirectToAction(nameof(Index));
        }

        #region API calls

        [HttpGet]
        public IActionResult GetInquiryList()
        {
            return Json(new { data = _inquiryHeaderRepository.GetAll() });
        }
        #endregion
    }
}
