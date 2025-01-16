using DirectFarm.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectFarm.Core.Entity
{
    public class WarehouseEntity
    {
        public Guid Id { get; set; } = Guid.Empty;
        public string Name { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public WarehouseManagerEntity? Manager { get; set; }
        public WarehouseEntity() { }
        public WarehouseEntity(Guid Id) 
        {
            this.Id = Id;
        }
        public WarehouseEntity(WarehouseModel model) 
        {
            Id = model.warehouse_id;
            Name = model.name;
            Location = model.location;
            Manager = new WarehouseManagerEntity(model.manager_id);
        }
        public static List<WarehouseEntity> toEntityList(List<WarehouseModel> list)
        {
            if (list == null || !list.Any())
                return new List<WarehouseEntity>();

            return list.Select(model => new WarehouseEntity
            {
                Id = model.warehouse_id,
                Name = model.name,
                Location = model.location,
                Manager = new WarehouseManagerEntity(model.manager_id)
            }).ToList();
        }
    }
}
