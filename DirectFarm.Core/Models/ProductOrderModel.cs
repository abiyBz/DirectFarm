using DirectFarm.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace DirectFarm.Core.Models
{
    public class ProductOrderModel
    {
        public Guid id { get; set; } = Guid.NewGuid();
        public Guid product_id { get; set; }
        //public virtual ProductModel? Product { get; set; } = null!;
        public Guid order_id { get; set; }
        //public virtual OrderModel? order { get; set; }
        public decimal quantity { get; set; }
        public decimal price_at_purchase { get; set; }
        public decimal amount { get; set; }

        public ProductOrderModel(ProductOrderEntity entity, Guid orderid) 
        {
            id = Guid.Empty;
            product_id = entity.Product.Id;
            order_id = orderid;
            quantity = entity.Quantity;
            price_at_purchase = entity.PriceAtPurchase;
            amount = entity.Amount;
        }
        public ProductOrderModel() { }
    }
}
