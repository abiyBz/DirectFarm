using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectFarm.Core.Models
{
    public class AdminModel
    {
        public Guid admin_id { get; set; } = Guid.Empty;
        public string name { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string password_hash { get; set; } = string.Empty;
        public string role { get; set; } = "standard_admin";
        public DateTime created_at { get; set; } = DateTime.UtcNow;
        public string? refresh_token { get; set; }
    }
}
