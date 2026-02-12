using System.Reflection;
using Infrastructure.Common.MediatR.Behaviours;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.MediatR;

public static class Configuration
{
    public static IServiceCollection AddMediator(
        this IServiceCollection services,
        Assembly assembly
    )
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));

        return services;
    }
    
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
}