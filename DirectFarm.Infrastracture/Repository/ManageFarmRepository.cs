using DirectFarm.Core.Contracts.Repositories;
using DirectFarm.Core.Entity;
using DirectFarm.Core.Models;
using DirectFarm.Infrastracture.Context;
using Infrastracture.Base;
using Infrastracture.Base.EF;
using Infrastracture.Base.EF.Minio;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Minio.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DirectFarm.Infrastracture.Repository
{
    public class ManageFarmRepository : GenericRepository, IManageDirectFarmRepository
    {
        private readonly MinioDocumentService _minioDocumentService;
        public ManageFarmRepository(DirectFarmContext context, MinioDocumentService minioDocumentService): base(context) 
        {
            _minioDocumentService = minioDocumentService;
        }
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
                    model.image = updateModel.image;
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

        public async Task<bool> SaveProductPic (ProductImageEntity entity)
        {
            var fileType = Path.GetExtension(entity.FileName)?.ToLower();
            DocumentBase documentBase = new DocumentBase
            {
                DocumentIdentifier = entity.Id,
                Extension = fileType,
                Bucket = "direct-farm",
                Content = Convert.FromBase64String(ConverStringToBase64(entity.Image)),
                ContentType = "image/" + fileType,
            };

            var product = await FindOneAsync<ProductModel>(x => x.product_id == entity.Id);

            // Ensure that the talent exists, and if the MinIO upload fails, return false
            if (product == null || !_minioDocumentService.UploadDocument(documentBase).Result.Data)
                return false;

            // Set picture and update timestamps
            product.image = entity.FileName;

            // Ensure both dates are set as UTC
            product.created_at = SetKindUtc(product.created_at); // Handle unspecified DateTimeKind

            // Update and save changes
            await UpdateAsync<ProductModel>(product);
            await UnitOfWork.SaveChanges(); // Make sure SaveChanges is asynchronous
            return true;
        }
        private static string ConverStringToBase64(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        private static DateTime SetKindUtc(DateTime dateTime)
        {
            return dateTime.Kind == DateTimeKind.Unspecified ?
                DateTime.SpecifyKind(dateTime, DateTimeKind.Utc) :
                dateTime.ToUniversalTime(); // Ensure it's converted if needed
        }
        public async Task<byte[]> GetProductPicture(Guid id)
        {
            var product = await FindOneAsync<ProductModel>(x => x.product_id == id);
            if (product != null && !string.IsNullOrWhiteSpace(product.image))
            {
                var fileType = Path.GetExtension(product.image)?.ToLower();
                var fileName = Path.GetFileName(product.image)?.TrimEnd('.');

                DocumentRequestBase documentRequestBase = new DocumentRequestBase()
                {
                    DocumentIdentifier = id,
                    FileName = fileName,
                    Bucket = "direct-farm",
                    Extension = fileType,
                };
                var img = _minioDocumentService.GetDocumentAsByteArray(documentRequestBase);
                return img.Result.Data.Content;            }

            else throw new Exception("No picture found");

        }
        
        public async Task<OrderEntity>  PlaceOrder(OrderEntity order) 
        {
            var products = new List<ProductModel>();
            var customer = await FindOneAsync<CustomerModel>(c => c.customer_id == order.customer.Id);
            if (order.ProductOrders == null || order.ProductOrders.Count == 0 || customer == null) throw new Exception("Neccessary Information is missing!"); 
            order.TotalAmount = 0;
            int num = 1;
            foreach(var item in order.ProductOrders) 
            {
                var product = await FindOneAsync<ProductModel>(x => x.product_id == item.Product.Id);
                if (product == null)
                {
                    throw new Exception("Product "+ num.ToString() + " doesnt exist!");
                }
                item.PriceAtPurchase = product.price_per_unit;
                item.Product = new ProductEntity(product);
                item.Amount = item.PriceAtPurchase * item.Quantity;
                order.TotalAmount += item.Amount;
                num++;
            }
            order.customer = new CustomerEntity(customer);
            var ordermodel = new OrderModel(order);
            if(order.Id != Guid.Empty) 
            {
                var orderdelete = await FindOneAsync<OrderModel>(x => x.order_id== order.Id);
                if (orderdelete != null) 
                {
                    if (orderdelete.isPayed()) throw new Exception("Payment already made");
                    await DeleteAsync<ProductOrderModel>(x => x.order_id == order.Id);
                    await UnitOfWork.SaveChanges();
                    await DeleteAsync<OrderModel>(orderdelete);
                    await UnitOfWork.SaveChanges();
                }
            }
            await AddAsync<OrderModel>(ordermodel);
            await UnitOfWork.SaveChanges();
            order.Id = ordermodel.order_id;
            var tasks = new List<Task>();
            foreach (var item in order.ProductOrders)
            {
                tasks.Add(AddAsync<ProductOrderModel>(new ProductOrderModel(item, order.Id)));
            }
            await Task.WhenAll(tasks);
            await UnitOfWork.SaveChanges();

            return order;
        }

        public async Task<bool> recordPayment(Guid Id, bool success) 
        {
            var order = await FindOneAsync<OrderModel>(x=> x.order_id == Id);
            if (order == null) throw new Exception("No such order made");
            if(success)order.Success();
            else order.failure();
            order.orderdate = SetKindUtc(order.orderdate);
            await UpdateAsync<OrderModel>(order);
            await UnitOfWork.SaveChanges();
            return true;
        }
    }
}
