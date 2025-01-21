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
    public class SaveManagerTokenCommand : IRequest<Response<WarehouseManagerEntity>>
    {
        public string Email { get; set; } = string.Empty;
        public string Refreshtoken { get; set; } = string.Empty;
        public SaveManagerTokenCommand(string email, string refreshtoken)
        {
            Email = email;
            Refreshtoken = refreshtoken;
        }
    }
}
