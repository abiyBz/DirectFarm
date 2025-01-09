﻿using DirectFarm.Core.Entity;
using Infrastracture.Base;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectFarm.Core.Contracts.Query
{
    public class GetProductQuery : IRequest<Response<ProductEntity>>
    {
        public Guid Id { get; set; }
        public GetProductQuery(Guid Id) 
        {
            this.Id = Id;
        }
    }
}
