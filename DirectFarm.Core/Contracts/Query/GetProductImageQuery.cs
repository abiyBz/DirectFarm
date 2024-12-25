using Infrastracture.Base;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectFarm.Core.Contracts.Query
{
    public class GetProductImageQuery : IRequest<Response<string>>
    {
        public Guid Id { get; set; }
        public GetProductImageQuery(Guid Id)
        {
            this.Id = Id;
        }
    }
}
