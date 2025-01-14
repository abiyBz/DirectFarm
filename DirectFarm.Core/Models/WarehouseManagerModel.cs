using DirectFarm.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectFarm.Core.Models
{
    public class WarehouseManagerModel
    {
        public Guid manager_id { get; set; } = Guid.Empty;
        public string name { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string phone { get; set; } = string.Empty;
        public string status { get; set; } = "active";
        public string? refresh_token { get; set; }
        public WarehouseManagerModel() { }
        public WarehouseManagerModel(WarehouseManagerEntity entity) 
        {
            manager_id = entity.Id;
            name = entity.Name;
            email = entity.Email;
            phone = entity.Phone;
            status = entity.Status;
        }
    }
}
