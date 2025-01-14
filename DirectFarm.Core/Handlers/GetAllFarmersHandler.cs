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
    internal class GetAllFarmersHandler : IRequestHandler<GetAllFarmersQuery, Response<List<FarmerEntity>>>
    {
        IManageDirectFarmRepository repository;
        ILogger<GetAllFarmersHandler> logger;

        public GetAllFarmersHandler(IManageDirectFarmRepository repository, ILogger<GetAllFarmersHandler> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        public async Task<Response<List<FarmerEntity>>> Handle(GetAllFarmersQuery request, CancellationToken cancellationToken)
        {
            var response = new Response<List<FarmerEntity>>();
            try
            {
                var farmers = await this.repository.GetAllFarmers();
                response.Data = FarmerEntity.toEntityList(farmers);
                return response;

            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                response.Message = ex.Message;
                response.Ex = ex;
                response.ResponseStatus = ResponseStatus.Error;
                response.Data = new List<FarmerEntity>();
                return response;
            }
        }
    }
}
