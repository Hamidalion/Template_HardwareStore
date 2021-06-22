using Microsoft.AspNetCore.Identity;

namespace Template_HardwareStore.PL.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
    }
}
