using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cineverse.Identity.Authentication;

public static class AuthenticationSetup
{
    public static bool EnableSecurity;

    public static IServiceCollection AddAuth(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        // Load authentication options from configuration and register them in DI
        var authenticationOptions = services.ConfigureAuthenticationOptions(configuration);
        
        // Store EnableSecurity flag for global use
        EnableSecurity = authenticationOptions.EnableSecurity;
        
        // Register authorization policies and JWT authentication
        services.ConfigurePolicies();
        services.AddJwtAuthentication(authenticationOptions);
        
        return services;
    }
}