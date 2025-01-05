using DirectFarm.Core.Entity;
using DirectFarm.Core.Models;
using Infrastracture.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectFarm.Core.Contracts.Repositories
{
    public interface IManageDirectFarmRepository : IRepository
    {
        public Task<ProductModel> SaveProduct(ProductEntity product);
        public Task<OrderModel> GetOrderforPayment(Guid Id);
        public Task<List<ProductModel>> GetAllProducts();
        public Task<bool> SaveProductPic(ProductImageEntity entity);
        public Task<byte[]> GetProductPicture(Guid id);
        public Task<OrderEntity> PlaceOrder(OrderEntity order);
        public Task<bool> recordPayment(Guid Id, bool success);
        public Task<CustomerModel> SaveCustomer(CustomerEntity entity);
        public Task<CustomerModel> SaveRefreshToken(string email, string refreshtoken);
        public Task<CustomerModel> RegsiterCustomer(CustomerEntity entity, string Refreshtoken);
        public Task<string> GetRefreshToken(string email);
    }
}
