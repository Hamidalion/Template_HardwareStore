using Template_HardwareStore.DAL.Context;
using Template_HardwareStore.DAL.Repository.Interface;
using Template_HardwareStore.Entities.Models;

namespace Template_HardwareStore.DAL.Repository
{
    public class InquiryDetailRepository : Repository<InquiryDetail>, IInquiryDetailRepository
    {
        private readonly ApplicationDbContext _db;

        public InquiryDetailRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
            _db = applicationDbContext;
        }

        public void Update(InquiryDetail inquiryDetail)
        {
            _db.InquiryDetails.Update(inquiryDetail);
        }
    }
}
