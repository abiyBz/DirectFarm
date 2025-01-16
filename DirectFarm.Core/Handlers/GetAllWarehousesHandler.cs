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
    internal class GetAllWarehousesHandler : IRequestHandler<GetAllWarehousesQuery, Response<List<WarehouseEntity>>>
    {
        IManageDirectFarmRepository repository;
        ILogger<GetAllWarehousesHandler> logger;

        public GetAllWarehousesHandler(IManageDirectFarmRepository repository, ILogger<GetAllWarehousesHandler> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        public async Task<Response<List<WarehouseEntity>>> Handle(GetAllWarehousesQuery request, CancellationToken cancellationToken)
        {
            var response = new Response<List<WarehouseEntity>>();
            try
            {
                var model = await repository.GetAllWarehouses();
                response.Data = WarehouseEntity.toEntityList(model.ToList());
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
