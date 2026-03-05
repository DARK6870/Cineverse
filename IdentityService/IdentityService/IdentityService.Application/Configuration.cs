using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json;
using FluentValidation;
using IdentityService.Application.Services.EmailVerification;
using IdentityService.Application.Services.Password;
using IdentityService.Application.Services.RefreshToken;
using IdentityService.Application.Services.Token;
using IdentityService.Mongo.Schemas.Enums;
using Infrastructure.Kafka;
using Infrastructure.MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;
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
            .AddHttpContextAccessor()
            // TODO move this to package
            ;

        return services;
    }
}