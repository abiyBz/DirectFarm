using DirectFarm.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectFarm.Core.Models
{
    public class WarehouseModel
    {
        public Guid warehouse_id { get; set; } = Guid.Empty;
        public string name { get; set; } = string.Empty;
        public string location { get; set; } = string.Empty;
        public Guid manager_id { get; set; }
        public WarehouseModel() { } 
        public WarehouseModel(WarehouseEntity entity) 
        {
            warehouse_id = entity.Id;
            name = entity.Name;
            location = entity.Location;
            manager_id = entity.Manager.Id;
        }
    }
}
