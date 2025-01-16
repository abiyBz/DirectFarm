using DirectFarm.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectFarm.Core.Entity
{
    public class WarehouseManagerEntity
    {
        public Guid Id { get; set; } = Guid.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Status { get; set; } = "active";
        //public string? RefreshToken { get; set; }
        public WarehouseManagerEntity() { }
        public WarehouseManagerEntity(WarehouseManagerModel model) 
        {
            Id = model.manager_id;
            Name = model.name;
            Email = model.email;
            Phone = model.phone;
            Status = model.status;
        }
        public WarehouseManagerEntity(Guid id)
        {
            Id = id;
        }
        public WarehouseManagerEntity(Guid id, string name, string email, string phone, string status)
        {
            Id = id;
            Name = name;
            Email = email;
            Phone = phone;
            Status = status;
        }
        
    }
}
