using DirectFarm.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectFarm.Core.Models
{
    public class OrderModel
    {
        public Guid order_id { get; set; } //= Guid.NewGuid();
        public Guid customer_id { get; set; }
        //public virtual CustomerModel? customer { get; set; } = null!;
        public decimal total_amount { get; set; }
        public string status { get; set; } = "pending";
        public DateTime orderdate { get; set; } //= DateTime.UtcNow;
        public DateTime? paymentdate { get; set; } //= DateTime.UtcNow;

        public OrderModel(OrderEntity entity) 
        {
            order_id = entity.Id;
            customer_id = entity.customer.Id;
            total_amount = entity.TotalAmount;
            status = entity.Status; 
            orderdate = DateTime.UtcNow;
            paymentdate = null;
        }
        public OrderModel() {}
        
        public bool isPayed() 
        {
            return status == "success";
        }
        public void Success() 
        {
            status = "success";
            paymentdate= DateTime.UtcNow;
        }
        public void failure()
        {
            status = "failed";
            paymentdate = null;
        }
    }
}
