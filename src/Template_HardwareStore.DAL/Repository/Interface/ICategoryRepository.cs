using Template_HardwareStore.Entities.Models;

namespace Template_HardwareStore.DAL.Repository.Interface
{
    public interface ICategoryRepository : IRepository<Category>
    {
        void Update(Category category);
    }
}
