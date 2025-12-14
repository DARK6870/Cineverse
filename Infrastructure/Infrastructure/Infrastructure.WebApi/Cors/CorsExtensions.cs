using Infrastructure.WebApi.Cors.CorsPolicies;
using Infrastructure.WebApi.Cors.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.WebApi.Cors;

public static class CorsExtensions
{
    public static IServiceCollection AddCorsPolicy(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var corsOptions = configuration
            .GetSection(nameof(CorsOptions))
            .Get<CorsOptions>() ?? throw new ArgumentNullException(nameof(CorsOptions));
        
        services.AddCors(options =>
        {
            options.AddPolicy(nameof(DefaultCorsPolicy), policy =>
            {
                if (corsOptions.UseCorsPolicy)
                    policy.AllowAnyOrigin();
                else
                    policy.WithOrigins(corsOptions.AllowedOrigins);
                
                policy
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });

        return services;
    }

    public static IApplicationBuilder UseCorsPolicy(this IApplicationBuilder app)
    {
        app.UseCors(nameof(DefaultCorsPolicy));
        return app;
    }
}