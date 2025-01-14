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
    internal class SaveFarmerHandler : IRequestHandler<SaveFarmerCommand, Response<FarmerEntity>>
    {
        IManageDirectFarmRepository repository;
        ILogger<SaveFarmerHandler> logger;

        public SaveFarmerHandler(IManageDirectFarmRepository repository, ILogger<SaveFarmerHandler> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        public async Task<Response<FarmerEntity>> Handle(SaveFarmerCommand request, CancellationToken cancellationToken)
        {
            var response = new Response<FarmerEntity>();
            try
            {
                response.Data = new FarmerEntity(await this.repository.SaveFarmer(request.entity));
                return response;

            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                response.Message = ex.Message;
                response.Ex = ex;
                response.ResponseStatus = ResponseStatus.Error;
                response.Data = new FarmerEntity();
                return response;
            }
        }
    }
}
