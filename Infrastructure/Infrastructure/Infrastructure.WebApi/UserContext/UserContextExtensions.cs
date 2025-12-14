using Infrastructure.WebApi.UserContext.Middlewares;
using Infrastructure.WebApi.UserContext.UserContext;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.WebApi.UserContext;

public static class UserContextExtensions
{
    public static IServiceCollection AddUserContext(this IServiceCollection services)
    {
        services.AddScoped<IUserContext, UserContext.UserContext>();
        
        return services;
    }
    
    public static IApplicationBuilder UseUserContextMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<UserContextMiddleware>();
        
        return app;
    }
}