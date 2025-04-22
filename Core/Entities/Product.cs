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
    }
}
