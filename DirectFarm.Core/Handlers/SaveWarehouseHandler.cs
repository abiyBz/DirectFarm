using DirectFarm.Core.Contracts.Command;
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
    internal class SaveWarehouseHandler : IRequestHandler<SaveWarehouseCommand, Response<WarehouseEntity>>
    {
        IManageDirectFarmRepository repository;
        ILogger<SaveWarehouseHandler> logger;

        public SaveWarehouseHandler(IManageDirectFarmRepository repository, ILogger<SaveWarehouseHandler> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        public async Task<Response<WarehouseEntity>> Handle(SaveWarehouseCommand request, CancellationToken cancellationToken)
        {
            var response = new Response<WarehouseEntity>();
            try
            {
                response.Data = new WarehouseEntity(await repository.SaveWarehouse(request.Entity));
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
