using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using Template_HardwareStore.DAL.Context;
using Template_HardwareStore.DAL.Repository.Interface;
using Template_HardwareStore.Entities.Models;
using Template_HardwareStore.Utility.Constants;

namespace Template_HardwareStore.DAL.Repository
{
    public class InquiryHeaderRepository : Repository<InquiryHeader>, IInquiryHeaderRepository
    {
        private readonly ApplicationDbContext _db;

        public InquiryHeaderRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
            _db = applicationDbContext;
        }
                
        public void Update(InquiryHeader inquiryHeader)
        {
            _db.InquiryHeaders.Update(inquiryHeader);
        }
    }
}
