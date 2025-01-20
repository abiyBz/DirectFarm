using DirectFarm.Core.Entity;
using DirectFarm.Core.Models;
using Infrastracture.Base;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectFarm.Core.Contracts.Query
{
    public class GetAllCustomersQuery : IRequest<Response<List<CustomerEntity>>>
    {
    }
}
