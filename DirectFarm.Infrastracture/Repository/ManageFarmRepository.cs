using DirectFarm.Core.Contracts.Repositories;
using DirectFarm.Core.Entity;
using DirectFarm.Core.Models;
using DirectFarm.Infrastracture.Context;
using Infrastracture.Base;
using Infrastracture.Base.EF;
using Infrastracture.Base.EF.Minio;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
