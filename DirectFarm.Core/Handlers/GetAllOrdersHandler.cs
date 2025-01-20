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
    public class GetAllOrdersHandler : IRequestHandler<GetAllOrdersQuery, Response<List<OrderEntity>>>
    {
        IManageDirectFarmRepository repository;
        ILogger<GetAllOrdersHandler> logger;

        public GetAllOrdersHandler(IManageDirectFarmRepository repository, ILogger<GetAllOrdersHandler> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        public async Task<Response<List<OrderEntity>>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
        {
            var response = new Response<List<OrderEntity>>();
            try
            {
                var orders = await this.repository.GetAllOrders();
                response.Data = OrderEntity.toEntityList(orders);
                return response;

            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                response.Message = ex.Message;
                response.Ex = ex;
                response.ResponseStatus = ResponseStatus.Error;
                response.Data = new List<OrderEntity>();
                return response;
            }
        }
    }
}
