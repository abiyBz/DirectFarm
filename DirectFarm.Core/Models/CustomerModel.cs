using DirectFarm.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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

        public CustomerModel() { }
        public CustomerModel(CustomerEntity entity) 
        {
            customer_id = entity.Id;
            name = entity.Name;
            email = entity.Email;
            phone = entity.Phone;
            address = entity.Address;
            registration_date = entity.RegistrationDate;
            status = entity.Status;
        }
        public CustomerModel(CustomerEntity entity, string Refreshtok)
        {
            customer_id = entity.Id;
            name = entity.Name;
            email = entity.Email;
            phone = entity.Phone;
            address = entity.Address;
            registration_date = entity.RegistrationDate;
            status = entity.Status;
            refresh_token = Refreshtok;
        }
    }
}
