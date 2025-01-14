using DirectFarm.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectFarm.Core.Models
{
    public class FarmerProductModel
    {
        public Guid id { get; set; } = Guid.Empty;
        public Guid farmer_id { get; set; }
        public Guid product_id { get; set; }
        public decimal quantity_available { get; set; } = 0;
        public Guid warehouse_id { get; set; }
        public DateTime added_at { get; set; } = DateTime.UtcNow;
        public FarmerProductModel() { }
        public FarmerProductModel(FarmerProductEntity entity) 
        {
            id = entity.Id;
            farmer_id = entity.Farmer.Id;
            product_id = entity.Product.Id;
            quantity_available = entity.QuantityAvailable;
            warehouse_id = entity.Warehouse.Id;
            added_at = DateTime.UtcNow;
        }

    }

}
