using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class ProductModel
    {
        public int? Id { get; set; }

        public string Name { get; set; } = null!;

        public decimal Price { get; set; }

        public DateTime? CreatedAt { get; set; }

        public string? Description { get; set; }

        public int Stock { get; set; }

        public bool IsAvailable { get; set; }
        public string? Category { get; set; } = null!;

        public string? SkuID { get; set; } = null!;

        public string? Supplier { get; set; } = null!;

        public decimal? Discount { get; set; }

        public string? ImageUrl { get; set; } = null!;

        public Int32? UserId { get; set; } = null!;

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

        public string? ImageUrl { get; set; } = null!;

    }

}
