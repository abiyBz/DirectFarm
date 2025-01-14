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
    internal class SaveManagerHandler : IRequestHandler<SaveManagerCommand, Response<WarehouseManagerEntity>>
    {
        IManageDirectFarmRepository repository;
        ILogger<SaveManagerHandler> logger;

        public SaveManagerHandler(IManageDirectFarmRepository repository, ILogger<SaveManagerHandler> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        public async Task<Response<WarehouseManagerEntity>> Handle(SaveManagerCommand request, CancellationToken cancellationToken)
        {
            var response = new Response<WarehouseManagerEntity>();
            try
            {
                response.Data = new WarehouseManagerEntity(await this.repository.SaveManager(request.entity));
                return response;

            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                response.Message = ex.Message;
                response.Ex = ex;
                response.ResponseStatus = ResponseStatus.Error;
                response.Data = new WarehouseManagerEntity();
                return response;
            }
        }
    }
}
