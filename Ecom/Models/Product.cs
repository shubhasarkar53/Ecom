using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecom.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string ProductName { get; set; } = string.Empty;

        [Required]
        public string CreatedBy { get; set; } = string.Empty;

        [Required]
        public DateTime CreatedOn { get; set; }

        public string? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        //Navigation Property
        public ProductItem ProductItem { get; set; } = null!;

    }
}