using DirectFarm.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectFarm.Core.Models
{
    public class FarmerModel
    {
        public Guid farmer_id { get; set; } = Guid.Empty;
        public string name { get; set; } = string.Empty;
        public string? email { get; set; }
        public string phone { get; set; } = string.Empty;
        public string location { get; set; } = string.Empty;
        public DateTime registration_date { get; set; } = DateTime.UtcNow;
        public string status { get; set; } = "active";
        public string? profile_picture { get; set; }

       public FarmerModel(FarmerEntity entity) 
        {
            farmer_id = entity.Id;
            name = entity.Name; 
            email = entity.Email;
            phone = entity.Phone;
            location = entity.Location;
            status = entity.Status;
        }
        public FarmerModel() { }
    }
}
