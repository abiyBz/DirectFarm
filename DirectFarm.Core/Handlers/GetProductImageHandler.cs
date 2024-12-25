using DirectFarm.Core.Contracts.Query;
using DirectFarm.Core.Contracts.Repositories;
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
    public class GetProductImageHandler : IRequestHandler<GetProductImageQuery, Response<string>>
    {
        IManageDirectFarmRepository repository;
        ILogger<GetProductImageHandler> logger;

        public GetProductImageHandler(IManageDirectFarmRepository repository, ILogger<GetProductImageHandler> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        public async Task<Response<string>> Handle(GetProductImageQuery request, CancellationToken cancellationToken)
        {
            var response = new Response<string>();
            try
            {
                response.Data = System.Text.Encoding.UTF8.GetString(await this.repository.GetProductPicture(request.Id));
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
            throw new NotImplementedException();
        }
    }
}
