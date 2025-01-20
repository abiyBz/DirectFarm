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
    internal class GetProductsBelowHandler : IRequestHandler<GetProductsBelowQuery,Response<List<ProductEntity>>>
    {
        IManageDirectFarmRepository repository;
        ILogger<GetProductsBelowHandler> logger;

        public GetProductsBelowHandler(IManageDirectFarmRepository repository, ILogger<GetProductsBelowHandler> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        public async Task<Response<List<ProductEntity>>> Handle(GetProductsBelowQuery request, CancellationToken cancellationToken)
        {
            var response = new Response<List<ProductEntity>>();
            try
            {
                var products = await this.repository.GetProductsBelowNum(request.quantity);
                response.Data = ProductEntity.toEntityList(products);
                return response;

            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                response.Message = ex.Message;
                response.Ex = ex;
                response.ResponseStatus = ResponseStatus.Error;
                response.Data = new List<ProductEntity>();
                return response;
            }
        }
    }
}
