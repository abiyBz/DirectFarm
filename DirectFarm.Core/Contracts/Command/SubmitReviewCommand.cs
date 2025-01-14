using DirectFarm.Core.Entity;
using Infrastracture.Base;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectFarm.Core.Contracts.Command
{
    public class SubmitReviewCommand : IRequest<Response<ReviewEntity>>
    {
        public ReviewEntity entity { get; set; }
        public SubmitReviewCommand(ReviewEntity entity)
        {
            this.entity = entity;
        }
    }
}
