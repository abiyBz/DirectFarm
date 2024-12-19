using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectFarm.Core.Entity
{
    public class FarmerProductEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public FarmerEntity? Farmer { get; set; } = null!;
        public ProductEntity? Product { get; set; } = null!;
        public decimal QuantityAvailable { get; set; } = 0;
        public Guid? WarehouseId { get; set; }
        public WarehouseEntity? Warehouse { get; set; }
        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
    }
}
