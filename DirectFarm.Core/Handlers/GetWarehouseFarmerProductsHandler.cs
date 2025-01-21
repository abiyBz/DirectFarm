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
    internal class GetWarehouseFarmerProductsHandler : IRequestHandler<GetWarehouseFarmerProductsQuery, Response<List<FarmerProductEntity>>>
    {
        IManageDirectFarmRepository repository;
        ILogger<GetWarehouseFarmerProductsHandler> logger;

        public GetWarehouseFarmerProductsHandler(IManageDirectFarmRepository repository, ILogger<GetWarehouseFarmerProductsHandler> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        public async Task<Response<List<FarmerProductEntity>>> Handle(GetWarehouseFarmerProductsQuery request, CancellationToken cancellationToken)
        {
            var response = new Response<List<FarmerProductEntity>>();
            try
            {
                response.Data = await repository.GetWarehouseFarmerProducts(request.WarehouseId);
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
