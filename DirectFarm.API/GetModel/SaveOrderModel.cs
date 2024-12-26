using DirectFarm.Core.Entity;

namespace DirectFarm.API.GetModel
{
    public class SaveOrderModel
    {
        public Guid Id { get; set; }
        public Guid CustomerID { get; set; }
        public List<ProductOrderModel> Details { get; set; }

        public OrderEntity toOrderEntity() 
        {
            var entity = new OrderEntity();
            entity.Id = Id;
            entity.customer = new CustomerEntity(CustomerID);
            //entity.TotalAmount = 0;

            entity.ProductOrders = new List<ProductOrderEntity>();

            foreach (var item in Details) 
            {
                entity.ProductOrders.Add(new ProductOrderEntity(item.ProductID, item.Quantity));
            }

            entity.Orderdate = DateTime.Now;

            return entity; 
        }
    }
     public class ProductOrderModel 
    {
        public Guid ProductID { get; set; }
        public decimal Quantity { get; set; }
        //public decimal Price{ get; set; }
        //public decimal Amount { get; set; }
    }
}
