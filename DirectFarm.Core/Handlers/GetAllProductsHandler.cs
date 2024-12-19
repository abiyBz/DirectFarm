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
    public class GetAllProductsHandler : IRequestHandler<GetAllProductsQuery, Response<List<ProductEntity>>>
    {
        IManageDirectFarmRepository repository;
        ILogger<GetAllProductsHandler> logger;

        public GetAllProductsHandler(IManageDirectFarmRepository repository, ILogger<GetAllProductsHandler> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        public async Task<Response<List<ProductEntity>>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var response = new Response<List<ProductEntity>>();
            try 
            {
                var products = await this.repository.GetAllProducts();
                response.Data = ProductEntity.ModeltoEntityList(products);
                return response;

            }
            catch(Exception ex) 
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
