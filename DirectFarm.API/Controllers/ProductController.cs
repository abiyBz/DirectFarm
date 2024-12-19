﻿using DirectFarm.Core.Contracts.Command;
using DirectFarm.Core.Contracts.Query;
using DirectFarm.Core.Entity;
using Infrastracture.Base;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DirectFarm.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IMediator mediator;

        public ProductController(ILogger<ProductController> logger, IMediator mediator)
        {
            _logger = logger;
            this.mediator = mediator;
        }

        [HttpPost("SaveProduct")]
        public async Task<Response<ProductEntity>> SaveProduct (ProductEntity product) 
        {
            var result = await this.mediator.Send(new SaveProductCommand(product));
            return result;
        }
        [HttpPost("GetAllProducts")]
        public async Task<Response<List<ProductEntity>>> GetAllProducts()
        {
            var result = await this.mediator.Send(new GetAllProductsQuery());
            return result;
        }
    }
}