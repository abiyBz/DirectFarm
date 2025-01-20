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
    public class GetProductsBelowQuery : IRequest<Response<List<ProductEntity>>>
    {
        public decimal quantity { get; set; }
        public GetProductsBelowQuery(decimal quantity)
        {
            this.quantity = quantity;
        }
    }
}
