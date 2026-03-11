using FluentValidation;
using IdentityService.Application.Services.EmailVerification;
using IdentityService.Application.Services.Password;
using IdentityService.Application.Services.RefreshToken;
using IdentityService.Application.Services.Token;
using Infrastructure.Kafka;
using Infrastructure.MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NotificationService.Client;

namespace IdentityService.Application;

public static class Configuration
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services
            .AddKafkaSettings(configuration)
            .AddMediator(typeof(Configuration).Assembly)
            .AddValidatorsFromAssembly(typeof(Configuration).Assembly)
            .AddScoped<ITokenService, TokenService>()
            .AddScoped<IRefreshTokenService, RefreshTokenService>()
            .AddScoped<IEmailVerificationService, EmailVerificationService>()
            .AddScoped<IRestorePasswordService, RestorePasswordService>()
            .AddNotificationServiceClient(configuration)
            ;

        return services;
    }
}