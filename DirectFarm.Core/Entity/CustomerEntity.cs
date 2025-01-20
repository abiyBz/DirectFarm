using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using DirectFarm.Core.Models;


namespace DirectFarm.Core.Entity
{
    public class CustomerEntity
    {
        public Guid Id { get; set; } = Guid.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;
        public string Status { get; set; } = "active";

        public CustomerEntity() { }
        public CustomerEntity(Guid Id, string Name, string Email, string Phone, string Address) 
        {
            this.Id = Id;
            this.Name = Name;
            this.Email = Email;
            this.Phone = Phone;
            this.Address = Address;
            this.RegistrationDate = DateTime.UtcNow;
            this.Status = "active";
        }
        public CustomerEntity(Guid id) 
        {
            Id = id;
        }
        public CustomerEntity(CustomerModel model) 
        {
            Id = model.customer_id;
            Name = model.name;
            Email = model.email;
            Phone = model.phone;
            Address = model.address;
            RegistrationDate = model.registration_date;
            Status = model.status;
            
        }
        public static List<CustomerEntity> toEntityList(List<CustomerModel> models)
        {
            List<CustomerEntity> entities = new List<CustomerEntity>();
            foreach (var model in models)
            {
                entities.Add(new CustomerEntity(model));
            }
            return entities;
        }
        //public string? RefreshToken { get; set; }
    }
}
