using DirectFarm.Core.Contracts.Command;
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
    internal class OrderPickedUpHandler : IRequestHandler<OrderPickedUpCommand, Response<bool>>
    {
        IManageDirectFarmRepository repository;
        ILogger<OrderPickedUpHandler> logger;

        public OrderPickedUpHandler(IManageDirectFarmRepository repository, ILogger<OrderPickedUpHandler> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        public async Task<Response<bool>> Handle(OrderPickedUpCommand request, CancellationToken cancellationToken)
        {
            var response = new Response<bool>();
            try
            {
                await this.repository.OrderPickedUp(request.OrderId);
                response.Data = true;
                return response;

            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                response.Message = ex.Message;
                response.Ex = ex;
                response.ResponseStatus = ResponseStatus.Error;
                response.Data = false;
                return response;
            }
        }
    }
}
