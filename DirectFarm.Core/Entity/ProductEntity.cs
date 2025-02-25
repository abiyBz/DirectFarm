using DirectFarm.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace DirectFarm.Core.Entity
{
    public class ProductEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Category { get; set; }
        public decimal PricePerUnit { get; set; }
        public string Unit { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string Status { get; set; } = "available";
        //public string? Image { get; set; }
        public string? NameAmharic { get; set; }
        public string? DescriptionAmharic { get; set; }
        public decimal PercentageDiscount { get; set; }
        [Range(0, 100, ErrorMessage = "Percentage discount must be between 0 and 100.")]
        public decimal? DiscountedPrice { get; set; }
        //public int quantity {  get; set; }
        public ProductEntity() { }
        public ProductEntity(ProductModel model) 
        {
            Id = model.product_id;
            Name = model.name;
            Description = model.description;
            Category = model.category;
            PricePerUnit = model.price_per_unit;
            Unit = model.unit; 
            CreatedAt = model.created_at;
            Status = model.status;
            //Image = model.image;
            NameAmharic = model.name_amharic;
            DescriptionAmharic = model.description_amharic;
            PercentageDiscount = model.discount *100;
            DiscountedPrice = model.price_per_unit - (model.discount * model.price_per_unit);
            //quantity = model.amount;
        }
        public ProductEntity(Guid id) 
        {
            Id = id;
        }
        public static List<ProductEntity> toEntityList(List<ProductModel> models)
        {
            if (models == null || !models.Any())
                return new List<ProductEntity>();

            return models.Select(model => new ProductEntity
            {
                Id = model.product_id,
                Name = model.name,
                NameAmharic = model.name_amharic,
                DescriptionAmharic = model.name_amharic,
                Description = model.description,
                Category = model.category,
                PricePerUnit = model.price_per_unit,
                Unit = model.unit,
                CreatedAt = model.created_at,
                Status = model.status,
                PercentageDiscount = model.discount * 100,
                DiscountedPrice = model.price_per_unit - (model.discount * model.price_per_unit),

            }).ToList();
        }
        public static List<ProductEntity> IdtoEntityList(List<Guid> id) 
        {
            var products = new List<ProductEntity>();

            foreach (var item in id) 
            {
                products.Add(new ProductEntity(item));
            }
            return products;
        }

    }
}
