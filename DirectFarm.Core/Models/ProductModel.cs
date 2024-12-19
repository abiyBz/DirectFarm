﻿using DirectFarm.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectFarm.Core.Models
{
    public class ProductModel
    {
        public Guid product_id { get; set; }
        public string name { get; set; } = string.Empty;
        public string? description { get; set; }
        public string? category { get; set; }
        public decimal price_per_unit { get; set; }
        public string unit { get; set; } = string.Empty;
        public DateTime created_at { get; set; } = DateTime.UtcNow;
        public string status { get; set; } = "available";

        public ProductModel(ProductEntity entity) 
        {
            product_id = entity.Id;
            name = entity.Name;
            description = entity.Description;
            category = entity.Category;
            price_per_unit = entity.PricePerUnit;
            unit = entity.Unit;
            created_at = entity.CreatedAt;
            status = entity.Status;
        }
        public ProductModel() { }
    }
}
