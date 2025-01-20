using Infrastracture.Base;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectFarm.Core.Contracts.Command
{
    public class OrderPickedUpCommand : IRequest<Response<bool>>
    {
        public Guid OrderId { get; set; }
        public OrderPickedUpCommand(Guid orderId)
        {
            this.OrderId = orderId;
        }
    }
}
