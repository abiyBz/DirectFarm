using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectFarm.Core.Models
{
    public class FarmerProductModel
    {
        public Guid id { get; set; } = Guid.NewGuid();
        public Guid farmer_id { get; set; }
        public FarmerModel Farmer { get; set; } = null!;
        public Guid product_id { get; set; }
        public ProductModel Product { get; set; } = null!;
        public decimal quantity_available { get; set; } = 0;
        public Guid? warehouse_id { get; set; }
        public WarehouseModel? Warehouse { get; set; }
        public DateTime added_at { get; set; } = DateTime.UtcNow;
    }
}
