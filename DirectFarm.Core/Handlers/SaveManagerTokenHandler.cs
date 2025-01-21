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
    internal class SaveManagerTokenHandler : IRequestHandler<SaveManagerTokenCommand, Response<WarehouseManagerEntity>>
    {
        IManageDirectFarmRepository repository;
        ILogger<SaveManagerTokenHandler> logger;

        public SaveManagerTokenHandler(IManageDirectFarmRepository repository, ILogger<SaveManagerTokenHandler> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        public async Task<Response<WarehouseManagerEntity>> Handle(SaveManagerTokenCommand request, CancellationToken cancellationToken)
        {
            var response = new Response<WarehouseManagerEntity>();
            try
            {
                response.Data = new WarehouseManagerEntity(await repository.SaveManagerRefreshToken(request.Email, request.Refreshtoken));
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
