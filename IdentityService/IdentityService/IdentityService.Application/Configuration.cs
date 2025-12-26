using FluentValidation;
using IdentityService.Application.Services.EmailVerification;
using IdentityService.Application.Services.RefreshToken;
using IdentityService.Application.Services.Token;
using Infrastructure.MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityService.Application;

public static class Configuration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services
            .AddMediator(typeof(Configuration).Assembly)
            .AddValidatorsFromAssembly(typeof(Configuration).Assembly)
            .AddScoped<ITokenService, TokenService>()
            .AddScoped<IRefreshTokenService, RefreshTokenService>()
            .AddScoped<IEmailVerificationService, EmailVerificationService>()
            ;
        
        return services;
    }
}