using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectFarm.Core.Models
{
    public class OrderModel
    {
        public Guid order_id { get; set; } = Guid.NewGuid();
        public Guid customer_id { get; set; }
        //public virtual CustomerModel? customer { get; set; } = null!;
        public decimal total_amount { get; set; }
        public string status { get; set; } = "pending";
        public DateTime orderdate { get; set; } = DateTime.UtcNow;
        public DateTime paymentdate { get; set; } = DateTime.UtcNow;
    }
}
