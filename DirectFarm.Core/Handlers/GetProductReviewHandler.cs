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
    internal class GetProductReviewHandler : IRequestHandler<GetProductReviewQuery, Response<List<ReviewEntity>>>
    {
        IManageDirectFarmRepository repository;
        ILogger<GetProductReviewHandler> logger;

        public GetProductReviewHandler(IManageDirectFarmRepository repository, ILogger<GetProductReviewHandler> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        public async Task<Response<List<ReviewEntity>>> Handle(GetProductReviewQuery request, CancellationToken cancellationToken)
        {
            var response = new Response<List<ReviewEntity>>();
            try
            {
                var reviews = await repository.GetProductReviews(request.ProductId);
                response.Data = ReviewEntity.toEntityList(reviews);
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
