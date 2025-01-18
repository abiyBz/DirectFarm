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
    internal class GetAllManagersHandler : IRequestHandler<GetAllManagersQuery, Response<List<WarehouseManagerEntity>>>
    {
        IManageDirectFarmRepository repository;
        ILogger<GetAllManagersHandler> logger;

        public GetAllManagersHandler(IManageDirectFarmRepository repository, ILogger<GetAllManagersHandler> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        public async Task<Response<List<WarehouseManagerEntity>>> Handle(GetAllManagersQuery request, CancellationToken cancellationToken)
        {
            var response = new Response<List<WarehouseManagerEntity>>();
            try
            {
                var model = await repository.GetAllWarehouseManagers();
                response.Data = WarehouseManagerEntity.toEntityList(model.ToList());
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
