using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Product
    {
        public int? Id { get; set; }

        public string? Name { get; set; } = null!;

        public decimal? Price { get; set; }

        public DateTime? CreatedAt { get; set; }

        public string? Description { get; set; }

        public int? Stock { get; set; }

        public bool? IsAvailable { get; set; }

        public string? Category { get; set; }

        public string? SkuID { get; set; } = null!;

        public string? Supplier { get; set; } = null!;

        public decimal? Discount { get; set; }

        public int? UserId { get; set; } = null!;
    }
}
