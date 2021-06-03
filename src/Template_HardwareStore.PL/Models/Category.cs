using System.ComponentModel.DataAnnotations;

namespace Template_HardwareStore.PL.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; } // Primary key

        public string Name { get; set; }

        public int DisplayOrder { get; set; }
    }
}
