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
    internal class GetOrderProductsHandler : IRequestHandler<GetOrderProductsQuery, Response<List<ProductEntity>>>
    {
        IManageDirectFarmRepository repository;
        ILogger<GetOrderProductsHandler> logger;

        public GetOrderProductsHandler(IManageDirectFarmRepository repository, ILogger<GetOrderProductsHandler> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        public async Task<Response<List<ProductEntity>>> Handle(GetOrderProductsQuery request, CancellationToken cancellationToken)
        {
            var response = new Response<List<ProductEntity>>();
            try
            {
                var products = await this.repository.GetOrderProducts(request.Id);
                response.Data = ProductEntity.toEntityList(products.ToList());
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
