﻿using System;
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
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;
        public string Status { get; set; } = "active";

        public CustomerEntity() { }
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
        //public string? RefreshToken { get; set; }
    }
}
