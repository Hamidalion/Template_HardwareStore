using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using Template_HardwareStore.Entities.Models;

namespace Template_HardwareStore.DAL.Repository.Interface
{
    public interface IProductRepository : IRepository<Product>
    {
        void Update(Product product);

        IEnumerable<SelectListItem> GetAllDropDawnList(string obj);
    }
}
