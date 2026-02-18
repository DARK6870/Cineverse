using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Authentication;

public static class AuthenticationSetup
{
    public static bool EnableSecurity;

    public static AuthenticationBuilder AddAuth(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var authenticationOptions = services.ConfigureAuthenticationOptions(configuration);
        
        EnableSecurity = authenticationOptions.EnableSecurity;
        
        services.ConfigurePolicies();
        return services.AddJwtAuthentication(authenticationOptions);
    }
}