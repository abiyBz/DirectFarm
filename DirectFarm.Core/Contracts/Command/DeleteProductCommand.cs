using Infrastracture.Base;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectFarm.Core.Contracts.Command
{
    public class DeleteProductCommand : IRequest<Response<bool>>
    {
        public Guid id { get; set; }
        public DeleteProductCommand(Guid id) {  this.id = id; }
    }
}
