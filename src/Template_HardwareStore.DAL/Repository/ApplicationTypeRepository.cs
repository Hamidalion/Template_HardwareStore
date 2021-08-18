using System.Linq;
using Template_HardwareStore.DAL.Context;
using Template_HardwareStore.DAL.Repository.Interface;
using Template_HardwareStore.Entities.Models;

namespace Template_HardwareStore.DAL.Repository
{
    public class ApplicationTypeRepository : Repository<ApplicationType>, IApplicationTypeRepository
    {
        private readonly ApplicationDbContext _db;

        public ApplicationTypeRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(ApplicationType applicationType)
        {
            var applicationFromDb = _db.ApplicationTypes.FirstOrDefault(u => u.Id == applicationType.Id);
            if (applicationFromDb != null)
            {
                applicationFromDb.Name = applicationType.Name;
            }
        }
    }
}
