using Template_HardwareStore.Entities.Models;

namespace Template_HardwareStore.DAL.Repository.Interface
{
    public interface IApplicationTypeRepository : IRepository<ApplicationType>
    {
        public void Update(ApplicationType applicationType);
    }
}
