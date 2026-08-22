using System.ComponentModel.DataAnnotations;

namespace Ecom.DTOs.Product
{
    public class ProductRequestDTO
    {
        [Required]
        [StringLength(255)]
        public string ProductName { get; set; } = string.Empty;

        [Required]
        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }
    }
}
