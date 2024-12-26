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
    public class RecordPaymentHandler : IRequestHandler<RecordPaymentCommand, Response<bool>>
    {
        IManageDirectFarmRepository repository;
        ILogger<RecordPaymentHandler> logger;

        public RecordPaymentHandler(IManageDirectFarmRepository repository, ILogger<RecordPaymentHandler> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        public async Task<Response<bool>> Handle(RecordPaymentCommand request, CancellationToken cancellationToken)
        {
            var response = new Response<bool>();
            try 
            {
                response.Message = "Payment Completed";
                response.Data = await repository.recordPayment(request.param, request.success);
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
