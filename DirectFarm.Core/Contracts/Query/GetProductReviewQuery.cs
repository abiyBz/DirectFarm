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
    public class GetProductReviewQuery : IRequest<Response<List<ReviewEntity>>>
    {
        public Guid ProductId { get; set; }
        public GetProductReviewQuery(Guid productId)
        {
            this.ProductId = productId;
        }
    }
}
