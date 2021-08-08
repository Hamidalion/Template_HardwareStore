using Microsoft.AspNetCore.Identity;

namespace Template_HardwareStore.Entities.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
    }
}
