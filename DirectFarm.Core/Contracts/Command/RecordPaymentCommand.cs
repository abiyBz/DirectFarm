using Infrastracture.Base;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectFarm.Core.Contracts.Command
{
    public class RecordPaymentCommand : IRequest<Response<bool>>
    {
        public Guid param {  get; set; }
        public bool success { get; set; }
        public RecordPaymentCommand(Guid param, bool success)
        {
            this.param = param;
            this.success = success;
        }   
    }
}
