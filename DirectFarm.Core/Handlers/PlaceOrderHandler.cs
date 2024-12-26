using DirectFarm.Core.Contracts.Command;
using DirectFarm.Core.Contracts.Query;
using DirectFarm.Core.Contracts.Repositories;
using DirectFarm.Core.Entity;
using Infrastracture.Base;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectFarm.Core.Handlers
{
    public class PlaceOrderHandler : IRequestHandler<PlaceOrderCommand, Response<OrderEntity>>
    {
        IManageDirectFarmRepository repository;
        ILogger<PlaceOrderHandler> logger;

        public PlaceOrderHandler(IManageDirectFarmRepository repository, ILogger<PlaceOrderHandler> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        public async Task<Response<OrderEntity>> Handle(PlaceOrderCommand request, CancellationToken cancellationToken)
        {
            var response = new Response<OrderEntity>();
            try
            {
                response.Data = await this.repository.PlaceOrder(request.param);
                return response;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                response.Message = ex.Message;
                response.Ex = ex;
                response.ResponseStatus = ResponseStatus.Error;
                return response;
            }
        }
    }
}
