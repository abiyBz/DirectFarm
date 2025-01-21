using DirectFarm.Core.Entity;
using Infrastracture.Base;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectFarm.Core.Contracts.Query
{
    public class GetManagersWarehouseQuery: IRequest<Response<List<WarehouseEntity>>>
    {
        public Guid manager_id { get; set; }
        public GetManagersWarehouseQuery(Guid manager_id)
        {
            this.manager_id = manager_id;
        }
    }
}
