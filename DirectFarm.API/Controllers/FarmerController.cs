using DirectFarm.Core.Contracts.Command;
using DirectFarm.Core.Contracts.Query;
using DirectFarm.Core.Entity;
using Infrastracture.Base;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DirectFarm.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FarmerController : ControllerBase
    {
        private readonly ILogger<FarmerController> _logger;
        private readonly IMediator mediator;
        public FarmerController(ILogger<FarmerController> logger, IMediator mediator)
        {
            _logger = logger;
            this.mediator = mediator;
        }

        [HttpPost("SaveFarmer")]
        public async Task<Response<FarmerEntity>> SaveFarmer(FarmerEntity entity)
        {
            var result = await this.mediator.Send(new SaveFarmerCommand(entity));
            return result;
            
        }
        [HttpGet("GetAllFarmers")]
        public async Task<Response<List<FarmerEntity>>> GetAllFarmerFarmers()
        {
            var result = await this.mediator.Send(new GetAllFarmersQuery());
            return result;
        }
    }
}
