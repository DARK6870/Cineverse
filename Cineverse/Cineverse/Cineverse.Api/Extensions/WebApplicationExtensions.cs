using Infrastructure.Context;
using Infrastructure.HealthCheck;
using Infrastructure.WebApi.GraphQl;

namespace Cineverse.Api.Extensions;

public static class WebApplicationExtensions
{
    public static void ConfigureWebApplication(this WebApplication app)
    {
        app.UseTraceIdMiddleware();
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapGraphQlApi();

        app.UseUserContextMiddleware();
        app.UseGraphQlStatusCodeMiddleware();
        app.MapHealthCheck();
    }
}