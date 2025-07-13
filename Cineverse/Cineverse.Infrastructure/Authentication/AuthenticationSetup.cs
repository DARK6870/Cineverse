using System.Text;
using Cineverse.Infrastructure.Common.Models.Options;
using Cineverse.Mongo.Schemas.Enums;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Cineverse.Infrastructure.Authentication;

public static class AuthenticationSetup
{
    public static bool EnableSecurity;

    public static readonly string AdminAccessPolicy = nameof(AdminAccessPolicy);
    public static readonly string ManagerAccessPolicy = nameof(ManagerAccessPolicy);
    public static readonly string UserAccessPolicy = nameof(UserAccessPolicy);

    private static readonly string[] AdminAccessPolicyRoles = [nameof(Role.Admin)];
    private static readonly string[] ManagerAccessPolicyRoles = [nameof(Role.Admin), nameof(Role.Manager)];
    private static readonly string[] UserAccessPolicyRoles = [nameof(Role.Admin), nameof(Role.Manager), nameof(Role.User)];

    public static IServiceCollection AddJwtAuthentication(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var authenticationOptions = configuration
                                        .GetSection(nameof(AuthenticationOptions))
                                        .Get<AuthenticationOptions>()
                                    ?? throw new InvalidOperationException("Missing AuthenticationOptions");
        
        services.AddSingleton(authenticationOptions);
        EnableSecurity = authenticationOptions.EnableSecurity;

        var jwtOptions = authenticationOptions.JwtOptions;
        
        services
            .AddAuthorizationBuilder()
            .AddPolicy(AdminAccessPolicy, policy => policy.RequireRole(AdminAccessPolicyRoles))
            .AddPolicy(ManagerAccessPolicy, policy => policy.RequireRole(ManagerAccessPolicyRoles))
            .AddPolicy(UserAccessPolicy, policy => policy.RequireRole(UserAccessPolicyRoles));

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtOptions.Issuer,

                    ValidateAudience = true,
                    ValidAudience = jwtOptions.Audience,

                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey))
                };
            });

        return services;
    }
}