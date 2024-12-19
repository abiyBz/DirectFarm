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
    internal class SaveProductHandler : IRequestHandler<SaveProductCommand, Response<ProductEntity>>
    {
        IManageDirectFarmRepository repository;
        ILogger<SaveProductHandler> logger;

        public SaveProductHandler(IManageDirectFarmRepository repository, ILogger<SaveProductHandler> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        public async Task<Response<ProductEntity>> Handle(SaveProductCommand request, CancellationToken cancellationToken)
        {
            var response = new Response<ProductEntity>();
            try 
            {
                response.Data = new ProductEntity(await this.repository.SaveProduct(request.Product));
                return response;
            }
            catch(Exception ex) 
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
