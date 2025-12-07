using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PosOptimizer.Infrastructure.DatabaseContext;
using PosOptimizer.Infrastructure.Repositories;

namespace PosOptimizer.Infrastructure.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<IAppDbContext, AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("SqlServer")));

        services.AddScoped<IPosRatioRepository, PosRatioRepository>();
        
        return services;
    }
}