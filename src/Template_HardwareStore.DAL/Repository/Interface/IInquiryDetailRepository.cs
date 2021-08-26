using Template_HardwareStore.Entities.Models;

namespace Template_HardwareStore.DAL.Repository.Interface
{
    public interface IInquiryDetailRepository : IRepository<InquiryDetail>
    {
        void Update(InquiryDetail inquiryDetail);
    }
}
