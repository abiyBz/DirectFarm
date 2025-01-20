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
    internal class GetAllCustomersHandler : IRequestHandler<GetAllCustomersQuery, Response<List<CustomerEntity>>>
    {
        IManageDirectFarmRepository repository;
        ILogger<GetAllCustomersHandler> logger;

        public GetAllCustomersHandler(IManageDirectFarmRepository repository, ILogger<GetAllCustomersHandler> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }
        public async Task<Response<List<CustomerEntity>>> Handle(GetAllCustomersQuery request, CancellationToken cancellationToken)
        {
            var response = new Response<List<CustomerEntity>>();
            try
            {
                var orders = await this.repository.GetAllCustomers();
                response.Data = CustomerEntity.toEntityList(orders);
                return response;

            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                response.Message = ex.Message;
                response.Ex = ex;
                response.ResponseStatus = ResponseStatus.Error;
                response.Data = new List<CustomerEntity>();
                return response;
            }
        }
    }
}
