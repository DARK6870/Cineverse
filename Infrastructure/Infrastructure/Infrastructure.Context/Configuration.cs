using Infrastructure.Context.Middlewares;
using Infrastructure.Context.UserContext;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Context;

public static class Configuration
{
    public static IServiceCollection AddUserContext(this IServiceCollection services)
    {
        services.AddScoped<IUserContext, UserContext.UserContext>();
        services.AddHttpContextAccessor();
        
        return services;
    }
    
    public static IApplicationBuilder UseUserContextMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<UserContextMiddleware>();
        
        return app;
    }
    
    public static IApplicationBuilder UseTraceIdMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<TraceIdMiddleware>();
        
        return app;
    }
}