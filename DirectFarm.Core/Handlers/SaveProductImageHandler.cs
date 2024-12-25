using DirectFarm.Core.Contracts.Command;
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
    public class SaveProductImageHandler : IRequestHandler<SaveProductImageCommand, Response<bool>>
    {
        IManageDirectFarmRepository repository;
        ILogger<SaveProductImageHandler> logger;

        public SaveProductImageHandler(IManageDirectFarmRepository repository, ILogger<SaveProductImageHandler> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        public async Task<Response<bool>> Handle(SaveProductImageCommand request, CancellationToken cancellationToken)
        {
            var response = new Response<bool>();
            try
            {
                //add the image to miniio get the location and add to the location variable of location
                response.Data = await this.repository.SaveProductPic(request.param);
                response.Message = "Product picture saved!";
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
