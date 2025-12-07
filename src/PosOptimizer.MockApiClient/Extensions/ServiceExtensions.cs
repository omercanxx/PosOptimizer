using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PosOptimizer.MockApiClient.Options;

namespace PosOptimizer.MockApiClient.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddMockApiClient(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MockApiOptions>(configuration.GetSection(MockApiOptions.SectionName));

        services.AddHttpClient<IMockApiClient, MockApiClient>();
        
        return services;
    }
}