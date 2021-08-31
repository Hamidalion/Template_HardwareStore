using System.Collections.Generic;

namespace Template_HardwareStore.Entities.Models.ViewModels
{
    public class InquiryViewModel
    {
        public IEnumerable<InquiryDetail> InquiryDetail { get; set; }

        public InquiryHeader InquiryHeader { get; set; }
    }
}
