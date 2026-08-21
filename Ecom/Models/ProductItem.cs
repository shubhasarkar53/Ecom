using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecom.Models
{
    public class ProductItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int ProductId { get; set; }

        [Required]
        public int Quantity { get; set; }
        
        //Navigation Property
        public Product Product { get; set; } = null!;
    }
}