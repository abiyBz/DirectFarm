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
    internal class GetManagersWarehouseHandler : IRequestHandler<GetManagersWarehouseQuery, Response<List<WarehouseEntity>>>
    {
        IManageDirectFarmRepository repository;
        ILogger<GetManagersWarehouseHandler> logger;

        public GetManagersWarehouseHandler(IManageDirectFarmRepository repository, ILogger<GetManagersWarehouseHandler> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        public async Task<Response<List<WarehouseEntity>>> Handle(GetManagersWarehouseQuery request, CancellationToken cancellationToken)
        {
            var response = new Response<List<WarehouseEntity>>();
            try
            {
                var result = await repository.GetManagersWarehouses(request.manager_id);
                response.Data = WarehouseEntity.toEntityList(result.ToList());
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
