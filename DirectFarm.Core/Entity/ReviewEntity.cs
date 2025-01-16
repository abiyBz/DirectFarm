using DirectFarm.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectFarm.Core.Entity
{
    public class ReviewEntity
    {
        //public Guid Id { get; set; } = Guid.Empty;
        //public Guid ProductId { get; set; }
        public ProductEntity Product { get; set; } = null!;
        //public Guid CustomerId { get; set; }
        public CustomerEntity Customer { get; set; } = null!;
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ReviewEntity() { }
        public ReviewEntity(ReviewModel model)
        {
            //this.Id = model.review_id;
            this.Product = new ProductEntity(model.product_id);
            this.Customer = new CustomerEntity(model.customer_id);
            this.Rating = model.rating;
            this.Comment = model.comment;
            this.CreatedAt = model.created_at;
        }

        public static List<ReviewEntity> toEntityList(List<ReviewModel> models)
        {
            if (models == null || !models.Any())
                return new List<ReviewEntity>();

            return models.Select(model => new ReviewEntity
            {
                //Id = model.review_id,
                Product = new ProductEntity(model.product_id),
                Customer = new CustomerEntity(model.customer_id),
                Rating = model.rating,
                Comment = model.comment,
                CreatedAt = model.created_at,
            }).ToList();
        }
    }
}
