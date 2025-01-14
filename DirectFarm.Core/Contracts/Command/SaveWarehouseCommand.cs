using DirectFarm.Core.Entity;
using Infrastracture.Base;
using MediatR;

namespace DirectFarm.Core.Contracts.Command
{
    public class SaveWarehouseCommand : IRequest<Response<WarehouseEntity>>
    {
        public WarehouseEntity Entity { get; }

        public SaveWarehouseCommand(WarehouseEntity entity)
        {
            Entity = entity;
        }
    }
}
