using System.Net;
using System.Text.Json;
using Infrastructure.Common.Exceptions.Base;
using Infrastructure.WebApi.GraphQl.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Infrastructure.WebApi.Rest.Middlewares;

public class RestApiExceptionHandlerMiddleware(
    RequestDelegate next,
    ILogger<RestApiExceptionHandlerMiddleware> logger
)
{
    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path.StartsWithSegments(GraphQlConstants.GraphQlPath))
        {
            await next(context);
            return;
        }
        
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        HttpStatusCode status;
        string message;

        switch (exception)
        {
            case BaseException baseEx:
                status = baseEx.StatusCode;
                message = baseEx.ErrorMessage;
                break;

            default:
                status = HttpStatusCode.InternalServerError;
                message = "An unexpected error occurred.";
                logger.LogError(exception, "Unhandled exception occurred");
                break;
        }

        context.Response.StatusCode = (int)status;

        var response = new
        {
            message,
            statusCode = (int)status,
            statusText = status.ToString()
        };

        var json = JsonSerializer.Serialize(response);
        await context.Response.WriteAsync(json);
    }
}