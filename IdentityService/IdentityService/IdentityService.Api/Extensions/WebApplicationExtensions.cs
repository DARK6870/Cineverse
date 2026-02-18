using Infrastructure.Context;
using Infrastructure.HealthCheck;
using Infrastructure.WebApi.GraphQl;
using Infrastructure.WebApi.Rest;

namespace IdentityService.Api.Extensions;

public static class WebApplicationExtensions
{
    public static void ConfigureWebApplication(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
            app.MapScalar();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseUserContextMiddleware();

        app.MapControllers();
        app.MapGraphQlApi();

        app.UseGraphQlStatusCodeMiddleware();
        app.UseRestApiExceptionHandlerMiddleware();

        app.MapHealthCheck();
    }
}