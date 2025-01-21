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
    internal class GetAllFarmerProductsHandler : IRequestHandler<GetAllFarmerProductsQuery, Response<List<FarmerProductEntity>>>
    {
        IManageDirectFarmRepository repository;
        ILogger<GetAllFarmerProductsHandler> logger;

        public GetAllFarmerProductsHandler(IManageDirectFarmRepository repository, ILogger<GetAllFarmerProductsHandler> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        public async Task<Response<List<FarmerProductEntity>>> Handle(GetAllFarmerProductsQuery request, CancellationToken cancellationToken)
        {
            var response = new Response<List<FarmerProductEntity>>();
            try
            {
                response.Data = await repository.GetAllFarmerProducts();
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
