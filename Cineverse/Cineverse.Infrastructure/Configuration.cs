using Cineverse.Infrastructure.Common.Behaviours;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Cineverse.Infrastructure;

public static class Configuration
{
    public static IServiceCollection AddPipelineBehaviours(
        this IServiceCollection services
    )
    {
        services
            .AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>))
            .AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>))
            ;

        return services;
    }
    
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services
    )
    {
        services.AddHttpContextAccessor();
        services.AddMemoryCache();

        return services;
    }
}