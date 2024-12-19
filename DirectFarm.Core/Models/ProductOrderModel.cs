using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectFarm.Core.Models
{
    public class ProductOrderModel
    {
        public Guid id { get; set; } = Guid.NewGuid();
        public Guid product_id { get; set; }
        public virtual ProductModel? Product { get; set; } = null!;
        public Guid order_id { get; set; }
        public virtual OrderModel? order { get; set; }
        public decimal quantity { get; set; }
        public decimal price_at_purchase { get; set; }
        public decimal amount { get; set; }
    }
}
