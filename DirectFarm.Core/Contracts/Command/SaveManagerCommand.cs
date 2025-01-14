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
    public class SaveManagerCommand : IRequest<Response<WarehouseManagerEntity>>
    {
        public WarehouseManagerEntity entity { get; set; }
        public SaveManagerCommand(WarehouseManagerEntity entity)
        {
            this.entity = entity;
        }
    }
}
