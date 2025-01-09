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
    public class GetProductHandler : IRequestHandler<GetProductQuery, Response<ProductEntity>>
    {
        IManageDirectFarmRepository repository;
        ILogger<GetProductHandler> logger;

        public GetProductHandler(IManageDirectFarmRepository repository, ILogger<GetProductHandler> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        public async Task<Response<ProductEntity>> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            var response = new Response<ProductEntity>();
            try
            {
                var model = await this.repository.GetProduct(request.Id);
                response.Data = new ProductEntity(model);
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
