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
    public class GetCustomerOrdersQuery : IRequest<Response<List<OrderEntity>>>
    {
        public Guid customerId { get; set; }
        public GetCustomerOrdersQuery (Guid customerId)
        {
            this.customerId = customerId;
        }
    }
}
