using Microsoft.AspNetCore.Mvc;
using Template_HardwareStore.DAL.Repository.Interface;

namespace Template_HardwareStore.PL.Controllers
{
    public class InquiryController : Controller
    {
        private readonly IInquiryHeaderRepository _inquiryHeaderRepository;
        private readonly IInquiryDetailRepository _inquiryDetailRepository;

        public InquiryController(IInquiryHeaderRepository inquiryHeaderRepository, IInquiryDetailRepository inquiryDetailRepository)
        {
            _inquiryHeaderRepository = inquiryHeaderRepository;
            _inquiryDetailRepository = inquiryDetailRepository;
        }

        public IActionResult Index()
        {
            return View();
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
