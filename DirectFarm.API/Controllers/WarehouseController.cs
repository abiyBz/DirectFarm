using DirectFarm.API.GetModel;
using DirectFarm.Core.Contracts.Command;
using DirectFarm.Core.Contracts.Query;
using DirectFarm.Core.Entity;
using Infrastracture.Base;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

namespace DirectFarm.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WarehouseController : ControllerBase
    {
        private readonly ILogger<WarehouseController> _logger;
        private readonly IMediator mediator;
        public WarehouseController(ILogger<WarehouseController> logger, IMediator mediator)
        {
            _logger = logger;
            this.mediator = mediator;
        }

        [HttpPost("RegisterFarmersProduct")]
        public async Task<Response<FarmerProductEntity>> RegisterFarmersProduct(FarmerProductEntity entity)
        {
            var result = await this.mediator.Send(new SaveFarmerProductCommand(entity));
            return result;
        }

        [HttpPost("RegisterWarehouse")]
        public async Task<Response<WarehouseEntity>> RegisterWarehouse(WarehouseEntity entity)
        {
            var result = await this.mediator.Send(new SaveWarehouseCommand(entity));
            return result;
        }

        [HttpPost("RegisterWarehouseManager")]
        public async Task<Response<WarehouseManagerEntity>> RegisterWarehouseManger(WarehouseManagerEntity entity)
        {
            var result = await this.mediator.Send(new SaveManagerCommand(entity));
            return result;
        }
        [HttpPost("GetFarmersProduct")]
        public async Task<Response<List<ProductEntity>>> GetFarmersProduct(BaseModel model)
        {
            var result = await this.mediator.Send(new GetFarmerProductsQuery(model.Id));
            return result;
        }
    }
}
