using Infrastructure.WebApi.Rest.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Scalar.AspNetCore;

namespace Infrastructure.WebApi.Rest;

public static class RestApiExtensions
{
    public static IServiceCollection AddRestApi(this IServiceCollection services)
    {
        services
            .AddOpenApi()
            .AddControllers();

        return services;
    }

    public static IEndpointRouteBuilder MapScalar(this IEndpointRouteBuilder app)
    {
        app.MapOpenApi();
        app.MapScalarApiReference(options =>
        {
            options.Theme = ScalarTheme.Purple;
        });

        return app;
    }
    
    public static IApplicationBuilder UseRestApiExceptionHandlerMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<RestApiExceptionHandlerMiddleware>();
        
        return app;
    }
}