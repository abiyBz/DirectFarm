using DirectFarm.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DirectFarm.Core.Entity
{
    public class FarmerEntity
    {
        public Guid Id { get; set; } = Guid.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string Phone { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        //public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;
        public string Status { get; set; } = "active";
        //public string? ProfilePicture { get; set; }

        public FarmerEntity() { }
        public FarmerEntity(Guid Id) 
        {
            this.Id = Id;
        }
        public FarmerEntity(FarmerModel model) 
        {
            Id = model.farmer_id;
            Name = model.name;
            Email = model.email;
            Phone = model.phone;
            Location = model.location;
            Status = model.status;
        }
        public static List<FarmerEntity> toEntityList(List<FarmerModel> models) 
        {
            if (models == null || !models.Any())
                return new List<FarmerEntity>();

            return models.Select(model => new FarmerEntity
            {
                Id = model.farmer_id,
                Name = model.name,
                Email = model.email,
                Phone= model.phone,
                Location = model.location,
                Status = model.status,
            }).ToList();
        }
    }
}
