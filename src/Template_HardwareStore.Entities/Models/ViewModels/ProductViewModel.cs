using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Template_HardwareStore.Entities.Models.ViewModels
{
    public class ProductViewModel
    {
        public Product Product { get; set; }

        public IEnumerable<SelectListItem> CategorySelectListItem { get; set; }

        public IEnumerable<SelectListItem> ApplicationTypeSelectListItem { get; set; }
    }
}
