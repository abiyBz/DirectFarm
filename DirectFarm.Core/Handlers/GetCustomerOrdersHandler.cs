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
    public class GetCustomerOrdersHandler : IRequestHandler<GetCustomerOrdersQuery, Response<List<OrderEntity>>>
    {
        IManageDirectFarmRepository repository;
        ILogger<GetCustomerOrdersHandler> logger;

        public GetCustomerOrdersHandler(IManageDirectFarmRepository repository, ILogger<GetCustomerOrdersHandler> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        public async Task<Response<List<OrderEntity>>> Handle(GetCustomerOrdersQuery request, CancellationToken cancellationToken)
        {
            var response = new Response<List<OrderEntity>>();
            try
            {
                response.Data = OrderEntity.toEntityList(await this.repository.GetCustomerOrders(request.customerId));
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
