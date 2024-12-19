using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectFarm.Core.Models
{
    public class WarehouseManagerModel
    {
        public Guid manager_id { get; set; } = Guid.NewGuid();
        public string name { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string phone { get; set; } = string.Empty;
        public string status { get; set; } = "active";
        public string? refresh_token { get; set; }
    }
}
