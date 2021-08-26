using Template_HardwareStore.Entities.Models;

namespace Template_HardwareStore.DAL.Repository.Interface
{
    public interface IInquiryHeaderRepository : IRepository<InquiryHeader>
    {
        void Update(InquiryHeader inquiryHeader);
    }
}
