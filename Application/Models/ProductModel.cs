using Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    // In ProductModel.cs
    public class ProductModel
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Product name is required")]
        public string Name { get; set; } = null!;

        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be positive")]
        public decimal Price { get; set; }

        public DateTime? CreatedAt { get; set; }

        public string? Description { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Stock cannot be negative")]
        public int Stock { get; set; }

        public bool IsAvailable { get; set; }

        public string? Category { get; set; }

        [Required(ErrorMessage = "SKU is required")]
        public string SkuID { get; set; } = null!;

        [Required(ErrorMessage = "Supplier is required")]
        public string Supplier { get; set; } = null!;

        [Range(0, 100, ErrorMessage = "Discount must be 0-100%")]
        public decimal? Discount { get; set; }

        [Required(ErrorMessage = "User ID is required")]
        public int UserId { get; set; }
    }

    public class UpdateProductModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public decimal? Price { get; set; }
        public string? Description { get; set; }
        public int? Stock { get; set; }
        public bool? IsAvailable { get; set; }
        public string? Category { get; set; }

        public string? SkuID { get; set; } = null!;

        public string? Supplier { get; set; } = null!;

        public decimal? Discount { get; set; } = null!;

    }

}
