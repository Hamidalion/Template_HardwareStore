using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Template_HardwareStore.PL.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; } // Primary key

        public string Name { get; set; }

        [DisplayName("Display Order")]
        public int DisplayOrder { get; set; }
    }
}
