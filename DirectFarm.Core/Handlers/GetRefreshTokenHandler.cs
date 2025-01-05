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
    public class GetRefreshTokenHandler : IRequestHandler<GetRefreshTokenQuery, Response<string>>
    {
        IManageDirectFarmRepository repository;
        ILogger<GetRefreshTokenHandler> logger;

        public GetRefreshTokenHandler(IManageDirectFarmRepository repository, ILogger<GetRefreshTokenHandler> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        public async Task<Response<string>> Handle(GetRefreshTokenQuery request, CancellationToken cancellationToken)
        {
            var response = new Response<string>();
            try
            {
                response.Data = await this.repository.GetRefreshToken(request.Email);
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
