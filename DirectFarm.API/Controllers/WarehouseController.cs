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

        
    }
}
