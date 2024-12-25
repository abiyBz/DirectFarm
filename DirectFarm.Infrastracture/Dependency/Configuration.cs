using DirectFarm.Core.Contracts.Repositories;
using DirectFarm.Infrastracture.Context;
using DirectFarm.Infrastracture.Repository;
using Infrastracture.Base.EF.Minio;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Minio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DirectFarm.Infrastracture.Dependency
{
    public static class Configuration
    {
        public static IServiceCollection AddInfrastractureConfiguration(this IServiceCollection services, IConfiguration config)
        {
            try
            {
                services.Configure<MinioServerConfigurationModel>(config.GetSection("Minio"));
                services.AddDbContext<DirectFarmContext>(options =>
                    options.UseNpgsql(config.GetConnectionString("DefaultConnection"),
                        b => b.MigrationsAssembly("DirectFarm.Infrastracture")));
                services.AddScoped<IManageDirectFarmRepository, ManageFarmRepository>();

                services.AddScoped<MinioDocumentService>();
                services.AddScoped<MinioClient>();

                var provider = services.BuildServiceProvider();

                var minioConfig = provider.GetRequiredService<IOptions<MinioServerConfigurationModel>>().Value;

                services.AddMinio(configureClient => configureClient
                    .WithEndpoint(minioConfig.EndPoint)
                    .WithCredentials(minioConfig.AccessKey, minioConfig.SecretKey)
                    .WithSSL(minioConfig.IsSecure)
                    .Build());
                return services;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred during service configuration: {ex.Message}");

                throw;
            }
        }
    }
}