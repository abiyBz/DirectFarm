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
    public class PlaceOrderCommand : IRequest<Response<OrderEntity>>
    {
        public OrderEntity param { get; set; }

        public PlaceOrderCommand (OrderEntity param)
        {
            this.param = param;
        }
    }
}
