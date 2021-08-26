using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Template_HardwareStore.Entities.Models
{
    public class InquiryDetail
    {
        [Key]
        public int Id { get; set; }

        public int InquiryHeaderId { get; set; }

        [ForeignKey("InquiryHeaderId")]
        public InquiryHeader InquiryHeader { get; set; }

        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }
    }
}
