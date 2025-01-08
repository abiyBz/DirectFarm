using DirectFarm.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DirectFarm.Core.Entity
{
    public class OrderEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public CustomerEntity customer { get; set; } = null!;
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = "pending";
        public DateTime Orderdate { get; set; } = DateTime.UtcNow;
        public DateTime? PaymentDate { get; set; }
        public List<ProductOrderEntity>? ProductOrders { get; set; }
        public OrderEntity() { }
        public OrderEntity(Guid id, CustomerEntity customer, decimal totalAmount, string status, DateTime orderdate, DateTime paymentDate, List<ProductOrderEntity> productOrders)
        {
            Id = id;
            this.customer = customer;
            TotalAmount = totalAmount;
            Status = status;
            Orderdate = orderdate;
            PaymentDate = paymentDate;
            ProductOrders = productOrders;
        }
        public OrderEntity(OrderModel model) 
        {
            Id = model.order_id;
            TotalAmount = model.total_amount;
            Status = model.status;
            Orderdate = model.orderdate;
            PaymentDate = model.paymentdate;
        }
        public OrderEntity(OrderModel model, CustomerEntity customer, List<ProductOrderEntity> productOrders) 
        {
            Id = model.order_id;
            TotalAmount = model.total_amount;
            Status = model.status;
            Orderdate = model.orderdate;
            PaymentDate = model.paymentdate;
            this.customer = customer;
            this.ProductOrders = productOrders;
        }
        public OrderEntity(OrderModel model, List<ProductOrderEntity> productOrders)
        {
            Id = model.order_id;
            TotalAmount = model.total_amount;
            Status = model.status;
            Orderdate = model.orderdate;
            PaymentDate = model.paymentdate;
            this.customer = new CustomerEntity();
            this.ProductOrders = productOrders;
        }
        public static List<OrderEntity> toEntityList(List<OrderModel> models) 
        {
            if (models == null || !models.Any())
                return new List<OrderEntity>();

            return models.Select(model => new OrderEntity
            {
                Id = model.order_id,
                TotalAmount = model.total_amount,
                Status = model.status,
                Orderdate = model.orderdate,
                PaymentDate = model.paymentdate
            }).ToList();
        }
    }
}
