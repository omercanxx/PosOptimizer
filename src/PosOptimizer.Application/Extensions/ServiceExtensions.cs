using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PosOptimizer.Application.Services;
using PosOptimizer.Application.Services.Abstractions;

namespace PosOptimizer.Application.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        return services
            .AddScoped<IPosRatioService, PosRatioService>();
    }
    
    public static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("Redis");
        });

        return services
            .AddScoped<IRedisCacheService, RedisCacheService>();
    }
}