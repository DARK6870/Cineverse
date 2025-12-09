using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Cineverse.Application;

public static class Configuration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Register Mediator
        services.AddMediatR(
            cfg => cfg.RegisterServicesFromAssembly(typeof(Configuration).Assembly)
        )
        .AddScoped<IMediator, Mediator>();
        
        // Register validators
        services.AddValidatorsFromAssembly(typeof(Configuration).Assembly);

        return services;
    }
}