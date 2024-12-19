﻿using DirectFarm.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectFarm.Core.Entity
{
    public class OrderEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public CustomerEntity? customer { get; set; } = null!;
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = "pending";
        public DateTime Orderdate { get; set; } = DateTime.UtcNow;
        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
        public List<ProductOrderEntity>? ProductOrders { get; set; }
        public OrderEntity() { }
        public OrderEntity(Guid id, CustomerEntity? customer, decimal totalAmount, string status, DateTime orderdate, DateTime paymentDate, List<ProductOrderEntity> productOrders)
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
    }
}
