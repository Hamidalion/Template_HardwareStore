using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using Template_HardwareStore.DAL.Context;
using Template_HardwareStore.DAL.Repository.Interface;
using Template_HardwareStore.Entities.Models;
using Template_HardwareStore.Utility.Constants;

namespace Template_HardwareStore.DAL.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _db;

        public ProductRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
            _db = applicationDbContext;
        }

        public IEnumerable<SelectListItem> GetAllDropDawnList(string obj)
        {
            if (obj == WebConstants.CategoryName)
            {
                return _db.Categories.Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString(),
                });
            }

            if (obj== WebConstants.ApplicationTypeName)
            {
                return _db.ApplicationTypes.Select(a => new SelectListItem
                {
                    Text = a.Name,
                    Value = a.Id.ToString(),
                });
            }
            return null;
        }

        public void Update(Product product)
        {
            _db.Products.Update(product);
        }
    }
}
