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
    public class SaveProductImageCommand : IRequest<Response<bool>>
    {
        public ProductImageEntity param;
        public SaveProductImageCommand(ProductImageEntity param) 
        {
            this.param = param;
        }
    }
}
