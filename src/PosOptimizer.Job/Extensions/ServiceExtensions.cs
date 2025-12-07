using Hangfire;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace PosOptimizer.Job.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddHangfireService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHangfire(config =>
        {
            config.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(configuration.GetConnectionString("SqlServer"));
        });

        services.AddHangfireServer();

        return services;
    }
}