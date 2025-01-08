using DirectFarm.Core.Contracts.Command;
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
    public class DeleteProductHandler : IRequestHandler<DeleteProductCommand, Response<bool>>
    {
        IManageDirectFarmRepository repository;
        ILogger<DeleteProductHandler> logger;
        public DeleteProductHandler(IManageDirectFarmRepository repository, ILogger<DeleteProductHandler> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        public async Task<Response<bool>> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var response = new Response<bool>();
            try
            {
                this.repository.DeleteProduct(request.id);
                response.Data = true;
                response.Message = "Product deleted successfuly!";
                return response;

            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                response.Message = ex.Message;
                response.Ex = ex;
                response.ResponseStatus = ResponseStatus.Error;
                response.Data = false;
                return response;
            }
        }
    }
}
