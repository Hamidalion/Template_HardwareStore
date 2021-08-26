using System.Linq;
using Template_HardwareStore.DAL.Context;
using Template_HardwareStore.DAL.Repository.Interface;
using Template_HardwareStore.Entities.Models;

namespace Template_HardwareStore.DAL.Repository
{
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {
        private readonly ApplicationDbContext _db;

        public ApplicationUserRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
