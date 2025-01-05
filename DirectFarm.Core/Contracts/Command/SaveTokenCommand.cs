using DirectFarm.Core.Entity;
using Infrastracture.Base;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectFarm.Core.Contracts.Command
{
    public class SaveTokenCommand : IRequest<Response<CustomerEntity>>
    {
        public string Email { get; set; } = string.Empty;
        public string Refreshtoken { get; set; } = string.Empty;
        public SaveTokenCommand(string email, string refreshtoken) 
        {
            Email = email;
            Refreshtoken = refreshtoken;
        }
    }
}
