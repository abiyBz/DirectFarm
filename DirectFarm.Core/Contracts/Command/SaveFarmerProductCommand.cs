using DirectFarm.Core.Entity;
using Infrastracture.Base;
using MediatR;

namespace DirectFarm.Core.Contracts.Command
{
    public class SaveFarmerProductCommand : IRequest<Response<FarmerProductEntity>>
    {
        public FarmerProductEntity Entity { get; }

        public SaveFarmerProductCommand(FarmerProductEntity entity)
        {
            Entity = entity;
        }
    }
}
