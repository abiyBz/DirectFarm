using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectFarm.Core.Models
{
    public class WarehouseModel
    {
        public Guid warehouse_id { get; set; } = Guid.NewGuid();
        public string name { get; set; } = string.Empty;
        public string location { get; set; } = string.Empty;
        public Guid? manager_id { get; set; }
        public WarehouseManagerModel? Manager { get; set; }
    }
}
