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
    public class GetWarehouseFarmerProductsQuery : IRequest<Response<List<FarmerProductEntity>>>
    {
        public Guid WarehouseId { get; set; }
        public GetWarehouseFarmerProductsQuery(Guid warehouseId)
        {
            WarehouseId = warehouseId;
        }
    }
}
