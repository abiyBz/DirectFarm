using DirectFarm.Core.Entity;
using Infrastracture.Base;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectFarm.Core.Contracts.Query
{
    public class GetOrderQuery : IRequest<Response<OrderEntity>>
    {
        public Guid orderID { get; set; }
        public GetOrderQuery(Guid orderID) 
        {
            this.orderID = orderID;
        }
    }
}
