using Microsoft.Extensions.DependencyInjection;

namespace Cineverse.Infrastructure;

public static class Configuration
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services
    )
    {
        services.AddHttpContextAccessor();
        services.AddMemoryCache();

        return services;
    }
}