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
    internal class SaveFarmerProductsHandler : IRequestHandler<SaveFarmerProductCommand, Response<FarmerProductEntity>>
    {
        IManageDirectFarmRepository repository;
        ILogger<SaveFarmerProductsHandler> logger;

        public SaveFarmerProductsHandler(IManageDirectFarmRepository repository, ILogger<SaveFarmerProductsHandler> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        public async Task<Response<FarmerProductEntity>> Handle(SaveFarmerProductCommand request, CancellationToken cancellationToken)
        {
            var response = new Response<FarmerProductEntity>();
            try
            {
                response.Data = new FarmerProductEntity(await repository.SaveFarmerProducts(request.Entity));
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
