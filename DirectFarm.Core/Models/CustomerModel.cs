using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectFarm.Core.Models
{
    public class CustomerModel
    {
        public Guid customer_id { get; set; } = Guid.NewGuid();
        public string name { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string phone { get; set; } = string.Empty;
        public string address { get; set; } = string.Empty;
        public DateTime registration_date { get; set; } = DateTime.UtcNow;
        public string status { get; set; } = "active";
        public string? refresh_token { get; set; } = string.Empty;
    }
}
