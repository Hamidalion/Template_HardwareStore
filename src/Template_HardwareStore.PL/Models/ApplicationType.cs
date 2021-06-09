using System.ComponentModel.DataAnnotations;

namespace Template_HardwareStore.PL.Models
{
    public class ApplicationType
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
