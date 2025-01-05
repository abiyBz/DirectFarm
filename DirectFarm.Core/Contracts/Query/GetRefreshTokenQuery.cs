using Infrastracture.Base;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectFarm.Core.Contracts.Query
{
    public class GetRefreshTokenQuery : IRequest<Response<string>>
    {
        public string Email { get; set; }
        public GetRefreshTokenQuery(string email) {  Email = email; }
    }
}
