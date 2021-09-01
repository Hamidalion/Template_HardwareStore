using Microsoft.AspNetCore.Mvc;
using Template_HardwareStore.DAL.Repository.Interface;
using Template_HardwareStore.Entities.Models.ViewModels;

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
            InquiryViewModel = new InquiryViewModel() {
                InquiryHeader = _inquiryHeaderRepository.FirstOrDefault(u => u.Id == id),
                InquiryDetail = _inquiryDetailRepository.GetAll(u => u.InquiryHeaderId == id, includeProperties: "Product") 
            };
            return View(InquiryViewModel);
        }

        #region API calls

        [HttpGet]
        public IActionResult GetInquiryList()
        {
            return Json( new { data = _inquiryHeaderRepository.GetAll()});
        }
        #endregion
    }
}
