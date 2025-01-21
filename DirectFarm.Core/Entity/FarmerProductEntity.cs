using DirectFarm.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectFarm.Core.Entity
{
    public class FarmerProductEntity
    {
        public Guid Id { get; set; } = Guid.Empty;
        public FarmerEntity? Farmer { get; set; } = null!;
        public ProductEntity? Product { get; set; } = null!;
        public decimal QuantityAvailable { get; set; } = 0;
        //public Guid? WarehouseId { get; set; }
        public WarehouseEntity? Warehouse { get; set; }
        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
        public FarmerProductEntity() { }
        public FarmerProductEntity(FarmerProductModel model) 
        {
            Id = model.id;
            Farmer = new FarmerEntity(model.farmer_id);
            Product = new ProductEntity(model.product_id);
            Warehouse = new WarehouseEntity(model.warehouse_id);
            QuantityAvailable = model.quantity_available;
            AddedAt = model.added_at;
        }
        public FarmerProductEntity(FarmerProductModel model, FarmerModel fmodel, WarehouseModel Wmodel, ProductModel pmodel)
        {
            Id = model.id;
            Farmer = new FarmerEntity(fmodel);
            Product = new ProductEntity(pmodel);
            Warehouse = new WarehouseEntity(Wmodel);
            QuantityAvailable = model.quantity_available;
            AddedAt = model.added_at;
        }
        public FarmerProductEntity (FarmerProductModel model, ProductModel pmodel)
        {
            Id = model.id;
            Product = new ProductEntity(pmodel);
            QuantityAvailable = model.quantity_available;
            AddedAt = model.added_at;
        }
        public FarmerProductEntity(FarmerProductModel model, FarmerModel fmodel, ProductModel pmodel)
        {
            Id = model.id;
            Farmer = new FarmerEntity(fmodel);
            Product = new ProductEntity(pmodel);
            QuantityAvailable = model.quantity_available;
            AddedAt = model.added_at;
        }
    }
}
