using ApiGateway.Cors;
using ApiGateway.Options;

namespace ApiGateway.Extensions;

public static class CorsDependencyInjectionExtensions
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