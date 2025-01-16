using DirectFarm.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace DirectFarm.Infrastracture.Context
{
    public class DirectFarmContext : DbContext
    {
        public DbSet<WarehouseManagerModel> WarehouseManagers { get; set; } = null!;
        public DbSet<WarehouseModel> Warehouses { get; set; } = null!;
        public DbSet<FarmerModel> Farmers { get; set; } = null!;
        public DbSet<ProductModel> Products { get; set; } = null!;
        public DbSet<FarmerProductModel> FarmerProducts { get; set; } = null!;
        public DbSet<CustomerModel> Customers { get; set; } = null!;
        public DbSet<ProductOrderModel> ProductOrders { get; set; } = null!;
        public DbSet<OrderModel> Orders { get; set; } = null!;
        public DbSet<ReviewModel> Reviews { get; set; } = null!;
        public DirectFarmContext(DbContextOptions<DirectFarmContext> options): base(options) { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.EnableDetailedErrors();
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder builder) 
        {
            SetTableName(builder);
            builder.Entity<ProductModel>().HasKey(x => x.product_id);
            builder.Entity<WarehouseManagerModel>().HasKey(x => x.manager_id);
            builder.Entity<WarehouseModel>().HasKey(x => x.warehouse_id);
            builder.Entity<FarmerModel>().HasKey(x => x.farmer_id);
            builder.Entity<FarmerProductModel>().HasKey(x => x.id);
            builder.Entity<CustomerModel>().HasKey(x => x.customer_id);
            builder.Entity<ProductOrderModel>().HasKey(x => x.id);
            builder.Entity<OrderModel>().HasKey(x => x.order_id);
            builder.Entity<ReviewModel>().HasKey(x => x.review_id);
            base.OnModelCreating(builder);
        }
        public void SetTableName(ModelBuilder builder) 
        {
            builder.Entity<WarehouseManagerModel>().ToTable("warehouse_manager");
            builder.Entity<WarehouseModel>().ToTable("warehouse");
            builder.Entity<FarmerModel>().ToTable("farmer");
            builder.Entity<ProductModel>().ToTable("products");
            builder.Entity<FarmerProductModel>().ToTable("farmer_product");
            builder.Entity<CustomerModel>().ToTable("customer");
            builder.Entity<ProductOrderModel>().ToTable("product_order");
            builder.Entity<OrderModel>().ToTable("order");
            builder.Entity<ReviewModel>().ToTable("reviews");
        }
    }
}
