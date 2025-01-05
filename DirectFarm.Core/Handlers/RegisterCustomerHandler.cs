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
    public class RegisterCustomerHandler : IRequestHandler<RegisterCustomerCommand, Response<CustomerEntity>>
    {
        IManageDirectFarmRepository repository;
        ILogger<RegisterCustomerHandler> logger;

        public RegisterCustomerHandler(IManageDirectFarmRepository repository, ILogger<RegisterCustomerHandler> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        public async Task<Response<CustomerEntity>> Handle(RegisterCustomerCommand request, CancellationToken cancellationToken)
        {
            var response = new Response<CustomerEntity>();
            try 
            {
                response.Data = new CustomerEntity(await repository.RegsiterCustomer(request.Customer, request.RefreshToken));
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
