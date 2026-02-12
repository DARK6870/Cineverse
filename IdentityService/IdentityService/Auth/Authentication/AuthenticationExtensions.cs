using System.Text;
using Auth.Models.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Auth.Authentication;

internal static class AuthenticationExtensions
{
    public static AuthenticationOptions ConfigureAuthenticationOptions(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var authenticationOptions = configuration
                                        .GetSection(nameof(AuthenticationOptions))
                                        .Get<AuthenticationOptions>()
                                    ?? throw new InvalidOperationException("Missing AuthenticationOptions");
        
        services.AddSingleton(authenticationOptions);
        return authenticationOptions;
    }

    public static IServiceCollection ConfigurePolicies(
        this IServiceCollection services
    )
    {
        services
            .AddAuthorizationBuilder()
            .AddDefaultPolicy("DefaultPolicy", policy => policy.RequireAuthenticatedUser())
            .AddPolicy(AuthenticationPolicies.AdminAccessPolicy, policy => policy.RequireRole(AuthenticationPolicies.AdminAccessPolicyRoles))
            .AddPolicy(AuthenticationPolicies.ManagerAccessPolicy, policy => policy.RequireRole(AuthenticationPolicies.ManagerAccessPolicyRoles))
            ;

        return services;
    }

    public static IServiceCollection AddJwtAuthentication(
        this IServiceCollection services,
        AuthenticationOptions authenticationOptions
    )
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = authenticationOptions.JwtOptions.Issuer,

                    ValidateAudience = true,
                    ValidAudience = authenticationOptions.JwtOptions.Audience,

                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationOptions.JwtOptions.SecretKey))
                };
            });

        return services;
    }
}