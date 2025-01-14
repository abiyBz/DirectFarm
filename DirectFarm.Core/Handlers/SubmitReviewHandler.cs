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
    internal class SubmitReviewHandler : IRequestHandler<SubmitReviewCommand, Response<ReviewEntity>>
    {
        IManageDirectFarmRepository repository;
        ILogger<SubmitReviewHandler> logger;

        public SubmitReviewHandler(IManageDirectFarmRepository repository, ILogger<SubmitReviewHandler> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        public async Task<Response<ReviewEntity>> Handle(SubmitReviewCommand request, CancellationToken cancellationToken)
        {
            var response = new Response<ReviewEntity>();
            try
            {
                var review = await this.repository.SaveReview(request.entity);
                response.Data = new ReviewEntity(review);
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
