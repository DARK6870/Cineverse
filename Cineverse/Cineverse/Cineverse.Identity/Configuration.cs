using Cineverse.Identity.Authentication;
using Cineverse.Identity.Services.Authentication;
using Cineverse.Identity.Services.EmailVerification;
using Cineverse.Identity.Services.RefreshToken;
using Cineverse.Identity.Services.RestorePassword;
using Cineverse.Identity.Services.TokenManagament;
using Cineverse.Identity.Services.UserContext;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cineverse.Identity;

public static class Configuration
{
    public static IServiceCollection AddIdentityServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services
            .AddAuth(configuration)
            .AddSingleton<IRefreshTokenService, RefreshTokenService>()
            .AddSingleton<IVerificationService, VerificationService>()
            .AddSingleton<IRestorePasswordService, RestorePasswordService>()
            .AddSingleton<ITokenManagementService, TokenManagementService>()
            .AddScoped<IAuthenticationService, AuthenticationService>()
            .AddScoped<IUserContext, UserContext>();

        return services;
    }
}