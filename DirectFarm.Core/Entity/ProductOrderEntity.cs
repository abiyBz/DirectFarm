using DirectFarm.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectFarm.Core.Entity
{
    public class ProductOrderEntity
    {
        //public Guid Id { get; set; } = Guid.NewGuid();
        public ProductEntity? Product { get; set; } = null!;
        //public OrderEntity? Order { get; set; }
        public decimal Quantity { get; set; }
        public decimal PriceAtPurchase{ get; set; }
        public decimal Amount { get; set; }
    }
}
