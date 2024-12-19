using DirectFarm.Core.Contracts.Repositories;
using DirectFarm.Infrastracture.Context;
using DirectFarm.Infrastracture.Repository;
using Infrastracture.Base.EF.Minio;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
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
                services.AddDbContext<DirectFarmContext>(options =>
                    options.UseNpgsql(config.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly("DirectFarm.Infrastracture")));
                services.AddScoped<IManageDirectFarmRepository, ManageFarmRepository>();
                var provider = services.BuildServiceProvider();

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
