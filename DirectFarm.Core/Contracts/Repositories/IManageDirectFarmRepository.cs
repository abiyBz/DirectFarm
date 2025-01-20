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
        public Task recordPayment(Guid Id, bool success);
        public Task<CustomerModel> SaveCustomer(CustomerEntity entity);
        public Task<CustomerModel> SaveRefreshToken(string email, string refreshtoken);
        public Task<CustomerModel> RegsiterCustomer(CustomerEntity entity, string Refreshtoken);
        public Task<string> GetRefreshToken(string email);
        public Task DeleteProduct(Guid productId);
        public Task<List<OrderModel>> GetCustomerOrders(Guid Id);
        public Task<ProductModel> GetProduct(Guid Id);
        public Task<List<ProductModel>> GetAvailableProducts();
        public Task<FarmerModel> SaveFarmer(FarmerEntity farmer);
        public Task<List<FarmerModel>> GetAllFarmers();
        public Task<WarehouseManagerModel> SaveManager(WarehouseManagerEntity entity);
        public Task<WarehouseModel> SaveWarehouse(WarehouseEntity entity);
        public Task<WarehouseModel> GetWarehouse(Guid Id);
        public Task<FarmerProductModel> SaveFarmerProducts(FarmerProductEntity entity);
        public Task<List<ProductModel>> GetOrderProducts(Guid orderId);
        public Task<ReviewModel> SaveReview(ReviewEntity entity);
        public Task<List<ProductModel>> GetFarmerProducts(Guid farmerId);
        public Task<List<ReviewModel>> GetProductReviews(Guid productId);
        public Task<List<WarehouseModel>> GetAllWarehouses();
        public Task<List<WarehouseManagerModel>> GetAllWarehouseManagers();
        public Task<List<OrderModel>> GetAllOrders();
        public Task<List<OrderModel>> GetCompltedOrders();
        public Task<List<CustomerModel>> GetAllCustomers();
        public Task<List<ProductModel>> GetProductsBelowNum(decimal num);
        public Task OrderPickedUp(Guid orderId);
    }
}
