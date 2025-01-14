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
    public class SaveFarmerCommand : IRequest<Response<FarmerEntity>>
    {
        public FarmerEntity entity { get; set; }
        public SaveFarmerCommand(FarmerEntity entity) {  this.entity = entity; }
    }
}
