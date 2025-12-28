using FluentValidation;
using Infrastructure.MediatR;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NotificationService.Client;

namespace Cineverse.Application;

public static class Configuration
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var assembly = typeof(Configuration).Assembly;

        services.AddMediator(assembly)
            .AddValidatorsFromAssembly(assembly)
            .AddNotificationServiceClient(configuration);

        return services;
    }
}