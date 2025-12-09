using Cineverse.Infrastructure.Cors.Models.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Cineverse.Infrastructure.Cors;

public static class CorsConfiguration
{
    public const string CorsPolicy = nameof(CorsPolicy);

    public static IServiceCollection AddCorsPolicy(
        this IServiceCollection services,
        IConfiguration configuration,
        IWebHostEnvironment environment
    )
    {
        var corsOptions = configuration
            .GetSection(nameof(CorsOptions))
            .Get<CorsOptions>() ?? throw new ArgumentNullException(nameof(CorsOptions));
        
        services.AddCors(options =>
        {
            options.AddPolicy(CorsPolicy, policy =>
            {
                if (environment.IsDevelopment())
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
}