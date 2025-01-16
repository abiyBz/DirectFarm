using DirectFarm.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectFarm.Core.Models
{
    public class ReviewModel
    {
        public Guid review_id { get; set; } = Guid.Empty;
        public Guid product_id { get; set; }
        public Guid customer_id { get; set; }
        public int rating { get; set; }
        public string? comment { get; set; }
        public DateTime created_at { get; set; } = DateTime.UtcNow;
        public ReviewModel() { }
        public ReviewModel(ReviewEntity entity) 
        {
            review_id = Guid.Empty;
            product_id = entity.Product.Id;
            customer_id = entity.Customer.Id;
            rating = entity.Rating;
            comment = entity.Comment;
            created_at = entity.CreatedAt;
        }
    }
}
