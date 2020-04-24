using Microsoft.Extensions.DependencyInjection;
using Popcore.API.Domain.Infrastructure;
using Popcore.API.Domain.Services;
using Popcore.API.Infrastructure.Providers;
using Popcore.API.Services;
using Popcore.API.Services.ProxyService.External;

namespace Popcore.API.Extensions
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddScopedServices(this IServiceCollection services)
        {
            // services dependancy injections
            services.AddScoped<IFoodProductService, FoodProductService>();
            services.AddScoped<IOpenFoodFactsProxyService, OpenFoodFactsProxyService>();
            services.AddScoped<IQueryBuilder, HttpQueryBuilder>();

            return services;
        }

        public static IServiceCollection AddTransientServices(this IServiceCollection services)
        {
            // Injecting dependancy in services so it can be accessible in classes.
            services.AddTransient<ICacheSettingProvider, CacheSettingProvider>();

            return services;
        }
    }
}