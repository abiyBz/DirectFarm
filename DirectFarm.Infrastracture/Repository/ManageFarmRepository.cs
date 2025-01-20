using DirectFarm.Core.Contracts.Repositories;
using DirectFarm.Core.Entity;
using DirectFarm.Core.Models;
using DirectFarm.Infrastracture.Context;
using Infrastracture.Base;
using Infrastracture.Base.EF;
using Infrastracture.Base.EF.Minio;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Minio.DataModel;
using Minio.DataModel.Notification;
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
        public ManageFarmRepository(DirectFarmContext context, MinioDocumentService minioDocumentService) : base(context)
        {
            _minioDocumentService = minioDocumentService;
        }
        public async Task<ProductModel> SaveProduct(ProductEntity product)
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
                var updateModel = await FindOneAsync<ProductModel>(x => x.product_id == product.Id);
                if (updateModel != null)
                {
                    model.product_id = product.Id;
                    model.image = updateModel.image;
                    model.amount = updateModel.amount;
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
        public async Task<OrderModel> GetOrderforPayment(Guid Id)
        {
            return await FindOneAsync<OrderModel>(x => x.order_id == Id);
        }
        public async Task<List<ProductModel>> GetAllProducts()
        {
            var response = await GetAllAsync<ProductModel>();
            return response.ToList();
        }

        public async Task<List<ProductModel>> GetAvailableProducts()
        {
            var response = await FindAsync<ProductModel>(x => x.status == "available" && x.amount > 0);
            return response.ToList();
        }

        public async Task<bool> SaveProductPic(ProductImageEntity entity)
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
                return img.Result.Data.Content;
            }

            else throw new Exception("No picture found");

        }

        public async Task<OrderEntity> PlaceOrder(OrderEntity order)
        {
            var products = new List<ProductModel>();
            var customer = await FindOneAsync<CustomerModel>(c => c.customer_id == order.customer.Id);
            if (order.ProductOrders == null || order.ProductOrders.Count == 0 || customer == null) throw new Exception("Neccessary Information is missing!");
            order.TotalAmount = 0;
            int num = 1;
            foreach (var item in order.ProductOrders)
            {
                var product = await FindOneAsync<ProductModel>(x => x.product_id == item.Product.Id);
                if (product == null)
                {
                    throw new Exception("Product " + num.ToString() + " doesnt exist!");
                }
                item.PriceAtPurchase = product.price_per_unit;
                item.Product = new ProductEntity(product);
                item.Amount = item.PriceAtPurchase * item.Quantity;
                if (product.amount < item.Quantity) throw new Exception($"There isn't enough {product.name} there is only {product.amount} {product.unit} left. Adjust your order accordingly and order again.; በቂ {product.name_amharic} አልተገኘም። ለአሁኑ የቀረው {product.amount} {product.unit} ብቻ ነው። ትዛዙን ያስተካክሉ እና እንደገና ያዘዙ።");
                order.TotalAmount += item.Amount;
                num++;
            }
            order.customer = new CustomerEntity(customer);
            var ordermodel = new OrderModel(order);
            if (order.Id != Guid.Empty)
            {
                var orderdelete = await FindOneAsync<OrderModel>(x => x.order_id == order.Id);
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

        public async Task recordPayment(Guid Id, bool success)
        {
            var order = await FindOneAsync<OrderModel>(x => x.order_id == Id);
            if (order == null) throw new Exception("No such order made");

            if (success)
            {
                order.Success();
                var productOrders = await FindAsync<ProductOrderModel>(x => x.order_id == order.order_id); // .ToListAsync() to ensure we're not holding onto the iterator

                foreach (var po in productOrders.ToList())
                {
                    var product = await FindOneAsync<ProductModel>(x => x.product_id == po.product_id);
                    if (product == null) continue;
                    product.amount -= po.quantity;
                    product.created_at = SetKindUtc(product.created_at);
                    await UpdateAsync<ProductModel>(product);
                }
            }
            else
            {
                order.failure();
            }

            order.orderdate = SetKindUtc(order.orderdate);
            await UpdateAsync<OrderModel>(order);
            await UnitOfWork.SaveChanges();
        }

        public async Task<CustomerModel> SaveCustomer(CustomerEntity entity)
        {
            var model = new CustomerModel(entity);
            if (entity.Id <= Guid.Empty)
            {
                model.registration_date = DateTime.UtcNow;
                await AddAsync<CustomerModel>(model);
                await UnitOfWork.SaveChanges();
            }
            else
            {
                var updateModel = await FindOneAsync<CustomerModel>(x => x.customer_id == entity.Id);
                if (updateModel != null)
                {
                    model.customer_id = entity.Id;
                    model.registration_date = SetKindUtc(model.registration_date);
                    await UpdateAsync<CustomerModel>(model);
                    await UnitOfWork.SaveChanges();
                }
                else
                {
                    throw new Exception("Customer doesn't exist!");
                }
            }
            return model;
        }

        public async Task<CustomerModel> RegsiterCustomer(CustomerEntity entity, string Refreshtoken)
        {
            var model = new CustomerModel(entity, Refreshtoken);
            entity.Id = Guid.Empty;
            model.registration_date = DateTime.UtcNow;
            await AddAsync<CustomerModel>(model);
            await UnitOfWork.SaveChanges();
            return model;
        }

        public async Task<CustomerModel> SaveRefreshToken(string email, string refreshtoken)
        {
            var model = await FindOneAsync<CustomerModel>(x => x.email == email);
            model.refresh_token = refreshtoken;
            model.registration_date = SetKindUtc(model.registration_date);
            await UpdateAsync<CustomerModel>(model);
            //await UnitOfWork.SaveChanges();
            return model;
        }

        public async Task<string> GetRefreshToken(string email)
        {
            var repsonse = await FindOneAsync<CustomerModel>(x => x.email == email);
            return repsonse.refresh_token;
        }
        public async Task DeleteProduct(Guid productId)
        {
            var product = await FindOneAsync<ProductModel>(x => x.product_id == productId);
            if (product == null) throw new Exception("Product not found!");
            await DeleteAsync<ProductModel>(product);
            await UnitOfWork.SaveChanges();
        }

        public async Task<List<OrderModel>> GetCustomerOrders(Guid Id)
        {
            var orderModels = await FindAsync<OrderModel>(x => x.customer_id == Id);
            var list = orderModels.ToList();
            return list;
        }
        public async Task<ProductModel> GetProduct(Guid Id)
        {
            return await FindOneAsync<ProductModel>(x => x.product_id == Id);
        }

        public async Task<FarmerModel> SaveFarmer(FarmerEntity farmer)
        {
            var model = new FarmerModel(farmer);
            if (farmer.Id <= Guid.Empty)
            {
                model.registration_date = DateTime.UtcNow;
                await AddAsync<FarmerModel>(model);
                await UnitOfWork.SaveChanges();
            }
            else
            {
                var updateModel = await FindOneAsync<FarmerModel>(x => x.farmer_id == farmer.Id);
                if (updateModel != null)
                {
                    model.profile_picture = updateModel.profile_picture;
                    model.registration_date = SetKindUtc(model.registration_date);
                    await UpdateAsync<FarmerModel>(model);
                    await UnitOfWork.SaveChanges();
                }
                else
                {
                    throw new Exception("Farmer doesn't exist!");
                }
            }
            return model;
        }
        public async Task<List<FarmerModel>> GetAllFarmers()
        {
            var farmers = await GetAllAsync<FarmerModel>();
            return farmers.ToList();
        }
        public async Task<WarehouseManagerModel> SaveManager(WarehouseManagerEntity entity)
        {
            var model = new WarehouseManagerModel(entity);
            if (entity.Id <= Guid.Empty)
            {
                await AddAsync<WarehouseManagerModel>(model);
                await UnitOfWork.SaveChanges();
            }
            else
            {
                var updateModel = await FindOneAsync<WarehouseManagerModel>(x => x.manager_id == model.manager_id);
                if (updateModel != null)
                {
                    await UpdateAsync<WarehouseManagerModel>(model);
                    await UnitOfWork.SaveChanges();
                }
                else
                {
                    throw new Exception("Warehouse manager doesn't exist!");
                }
            }
            return model;
        }
        public async Task<WarehouseModel> SaveWarehouse(WarehouseEntity entity)
        {
            var model = new WarehouseModel(entity);
            if (entity.Id <= Guid.Empty)
            {
                await AddAsync<WarehouseModel>(model);
                await UnitOfWork.SaveChanges();
            }
            else
            {
                var updateModel = await FindOneAsync<WarehouseModel>(x => x.warehouse_id == model.warehouse_id);

                if (updateModel != null)
                {
                    await UpdateAsync<WarehouseModel>(model);
                    await UnitOfWork.SaveChanges();
                }
                else
                {
                    throw new Exception("Warehouse doesn't exist!");
                }
            }
            return model;
        }
        public async Task<WarehouseModel> GetWarehouse(Guid Id)
        {
            return await FindOneAsync<WarehouseModel>(x => x.warehouse_id == Id);
        }
        public async Task<FarmerProductModel> SaveFarmerProducts(FarmerProductEntity entity)
        {
            var model = new FarmerProductModel(entity);

            if (entity.Id > Guid.Empty)
            {
                var updateModel = await FindOneAsync<FarmerProductModel>(x => x.id == model.id);

                if (updateModel != null)
                {

                    var productrm = await FindOneAsync<ProductModel>(x => x.product_id == updateModel.product_id);
                    productrm.created_at = SetKindUtc(productrm.created_at);
                    productrm.amount -= model.quantity_available;
                    await UpdateAsync<ProductModel>(productrm);
                    await DeleteAsync<FarmerProductModel>(updateModel);
                }
            }
            model.id = Guid.Empty;
            var product = await FindOneAsync<ProductModel>(x => x.product_id == model.product_id);
            product.created_at = SetKindUtc(product.created_at);
            product.amount += model.quantity_available;
            await UpdateAsync<ProductModel>(product);
            await AddAsync<FarmerProductModel>(model);
            await UnitOfWork.SaveChanges();
            return model;
        }
        public async Task<List<ProductModel>> GetOrderProducts(Guid orderId)
        {
            var order = await FindOneAsync<OrderModel>(x => x.order_id == orderId);
            var productOrders = await FindAsync<ProductOrderModel>(x => x.order_id == orderId);
            var products = new List<ProductModel>();
            foreach (var item in productOrders.ToList())
            {
                var product = await FindOneAsync<ProductModel>(x => x.product_id == item.product_id);
                if (product != null) products.Add(product);
            }
            return products;
        }
        public async Task<List<ProductModel>> GetFarmerProducts(Guid farmerId)
        {
            var farmerProducts = await FindAsync<FarmerProductModel>(x => x.farmer_id == farmerId);
            var products = new List<ProductModel>();
            foreach (var item in farmerProducts.ToList())
            {
                var product = await FindOneAsync<ProductModel>(x => x.product_id == item.product_id);
                if (product != null) products.Add(product);
            }
            return products;
        }
        public async Task<ReviewModel> SaveReview(ReviewEntity entity)
        {
            var model = new ReviewModel(entity);
            var review = await FindOneAsync<ReviewModel>(x => x.product_id == model.product_id && x.customer_id == model.product_id );
            if (review != null)
            {
                await DeleteAsync<ReviewModel>(review);
            }
            model.review_id = Guid.Empty;
            var orders = await FindAsync<OrderModel>(x => x.customer_id == entity.Customer.Id);
            if (orders == null) throw new Exception("Customer hasn't madeany orders!");
            var productOrders = new List<ProductOrderModel>();
            foreach (var item in orders.ToList())
            {
                productOrders.Add(await FindOneAsync<ProductOrderModel>(x => x.order_id == item.order_id && x.product_id == entity.Product.Id));
                if (productOrders.Count > 0) break;
            }
            if (productOrders.Count < 1) throw new Exception("Customer hasn't bought the product!");
            model.created_at = DateTime.UtcNow;
            await AddAsync<ReviewModel>(model);
            await UnitOfWork.SaveChanges();
            return model;
        }
        public async Task<List<ReviewModel>> GetProductReviews(Guid productId)
        {
            var reviews = await FindAsync<ReviewModel>(x => x.product_id == productId);
            return reviews.ToList();
        }
        public async Task<List<WarehouseModel>> GetAllWarehouses() 
        {
            var warehouses = await GetAllAsync<WarehouseModel>();
            return warehouses.ToList();
        }
        public async Task<List<WarehouseManagerModel>> GetAllWarehouseManagers()
        {
            var managers = await GetAllAsync<WarehouseManagerModel>();
            return managers.ToList();
        }
        public async Task<List<OrderModel>> GetAllOrders()
        {
            var orders = await GetAllAsync<OrderModel>();
            return orders.ToList();
        }
        public async Task<List<OrderModel>> GetCompltedOrders()
        {
            var orders = await FindAsync<OrderModel>(x => x.status == "success");
            return orders.ToList();
        }
        public async Task<List<CustomerModel>> GetAllCustomers()
        {
            var customers = await GetAllAsync<CustomerModel>();
            return customers.ToList();
        }
        public async Task<List<ProductModel>> GetProductsBelowNum(decimal num) 
        {
            var products = await FindAsync<ProductModel>(x => x.amount < num);
            return products.ToList();
        }
        public async Task OrderPickedUp(Guid orderId)
        {
            var order = await FindOneAsync<OrderModel>(x => x.order_id == orderId);
            if (order == null) throw new Exception("No such order made");
            if (order.status == "failed") 
            {
                var productOrders = await FindAsync<ProductOrderModel>(x => x.order_id == order.order_id); // .ToListAsync() to ensure we're not holding onto the iterator

                foreach (var po in productOrders.ToList())
                {
                    var product = await FindOneAsync<ProductModel>(x => x.product_id == po.product_id);
                    if (product == null) continue;
                    product.amount -= po.quantity;
                    product.created_at = SetKindUtc(product.created_at);
                    await UpdateAsync<ProductModel>(product);
                }
            }
            order.status = "picked up";
            order.orderdate = SetKindUtc(order.orderdate);
            var paymentdate = order.paymentdate?? DateTime.UtcNow;
            order.paymentdate = SetKindUtc(paymentdate);
            await UpdateAsync<OrderModel>(order);
            await UnitOfWork.SaveChanges();
        }
    }
    }
