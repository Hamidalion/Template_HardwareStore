using System.Collections.Generic;

namespace Template_HardwareStore.PL.Models.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<Product> Products { get; set; }

        public IEnumerable<Category> Categories { get; set; }

    }
}
