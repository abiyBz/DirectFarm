using DirectFarm.Core.Contracts.Command;
using DirectFarm.Core.Contracts.Query;
using DirectFarm.Core.Contracts.Repositories;
using DirectFarm.Core.Entity;
using Infrastracture.Base;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectFarm.Core.Handlers
{
    public class GetOrderHandler : IRequestHandler<GetOrderQuery, Response<OrderEntity>>
    {
        IManageDirectFarmRepository repository;
        ILogger<GetOrderHandler> logger;

        public GetOrderHandler(IManageDirectFarmRepository repository, ILogger<GetOrderHandler> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        public async Task<Response<OrderEntity>> Handle(GetOrderQuery request, CancellationToken cancellationToken)
        {
            var response = new Response<OrderEntity>();
            try
            {
                var Order = await this.repository.GetOrderforPayment(request.orderID);
                if (Order == null) throw new Exception("No such order Found!");
                response.Data = new OrderEntity(Order);
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
