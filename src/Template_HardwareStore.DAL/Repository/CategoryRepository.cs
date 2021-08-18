using System.Linq;
using Template_HardwareStore.DAL.Context;
using Template_HardwareStore.DAL.Repository.Interface;
using Template_HardwareStore.Entities.Models;

namespace Template_HardwareStore.DAL.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _db;

        public CategoryRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
            _db = applicationDbContext;
        }

        public void Update(Category category)
        {
            var categoryFromDb = _db.Categories.FirstOrDefault(u => u.Id == category.Id);
            if (categoryFromDb != null)
            {
                categoryFromDb.Name = category.Name;
                categoryFromDb.DisplayOrder = category.DisplayOrder;
            }
        }
    }
}
