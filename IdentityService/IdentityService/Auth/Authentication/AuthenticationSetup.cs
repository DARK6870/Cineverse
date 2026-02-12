using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Authentication;

public static class AuthenticationSetup
{
    public static bool EnableSecurity;

    public static IServiceCollection AddAuth(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var authenticationOptions = services.ConfigureAuthenticationOptions(configuration);
        
        EnableSecurity = authenticationOptions.EnableSecurity;
        
        services.ConfigurePolicies();
        services.AddJwtAuthentication(authenticationOptions);
        
        return services;
    }
}