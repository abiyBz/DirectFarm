using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectFarm.Core.Models
{
    public class ReviewModel
    {
        public Guid review_id { get; set; } = Guid.NewGuid();
        public Guid product_id { get; set; }
        public ProductModel Product { get; set; } = null!;
        public Guid customer_id { get; set; }
        public CustomerModel Customer { get; set; } = null!;
        public int rating { get; set; }
        public string? comment { get; set; }
        public DateTime created_at { get; set; } = DateTime.UtcNow;
    }
}
