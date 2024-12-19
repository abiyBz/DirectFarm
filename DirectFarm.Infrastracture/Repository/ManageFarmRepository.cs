using DirectFarm.Core.Contracts.Repositories;
using DirectFarm.Core.Entity;
using DirectFarm.Core.Models;
using DirectFarm.Infrastracture.Context;
using Infrastracture.Base.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectFarm.Infrastracture.Repository
{
    public class ManageFarmRepository : GenericRepository, IManageDirectFarmRepository
    {
        public ManageFarmRepository(DirectFarmContext context): base(context) { }
        public async Task<ProductModel>  SaveProduct(ProductEntity product) 
        {
            var model = new ProductModel(product);
            if (product.Id <= Guid.Empty)
            {
                model.created_at = DateTime.UtcNow;
                await AddAsync<ProductModel>(model);
                await UnitOfWork.SaveChanges();
            }
            else 
            {
                var updateModel = await FindOneAsync<ProductModel>(x=> x.product_id == product.Id);
                if (updateModel != null) 
                {
                    model.product_id = product.Id;
                    await UpdateAsync<ProductModel>(model);
                    await UnitOfWork.SaveChanges();
                }
                else
                {
                    throw new Exception("Product doesn't exist!");
                }
            }
            return model;
        }
        public async Task<OrderModel> GetOrderforPayment (Guid Id) 
        {
            return await FindOneAsync<OrderModel>(x => x.order_id == Id);
        }
        public async Task<List<ProductModel>> GetAllProducts() 
        {
            var response = await GetAllAsync<ProductModel>();
            return response.ToList();
        }
    }


}
