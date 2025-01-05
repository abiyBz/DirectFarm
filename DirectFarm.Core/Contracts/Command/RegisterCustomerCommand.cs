using DirectFarm.Core.Entity;
using Infrastracture.Base;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectFarm.Core.Contracts.Command
{
    public class RegisterCustomerCommand : IRequest<Response<CustomerEntity>>
    {
        public CustomerEntity Customer { get; set; }
        public string RefreshToken { get; set; }
        public RegisterCustomerCommand(CustomerEntity customer,  string refreshToken)
        {
            Customer = customer;
            RefreshToken = refreshToken;
        }
    }
}
