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
    public class SaveProductCommand : IRequest<Response<ProductEntity>>
    {
        public ProductEntity Product { get; set; }
        public SaveProductCommand(ProductEntity product) { this.Product = product; }
    }
}
