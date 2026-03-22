using System.Net;
using System.Text.Json;
using FluentValidation;
using Infrastructure.Common.Exceptions.Base;
using Infrastructure.WebApi.GraphQl.Constants;
using Infrastructure.WebApi.Rest.Models;
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
        string[] validationErrors = [];

        var baseException = exception.GetBaseException();
        switch (baseException)
        {
            case BaseException ex:
                status = ex.StatusCode;
                message = ex.ErrorMessage;
                break;
            
            case ValidationException ex:
                status = HttpStatusCode.BadRequest;
                message = "Validation Failed";
                validationErrors = ex.Errors.Select(x => x.ErrorMessage).ToArray();
                break;
            
            default:
                status = HttpStatusCode.InternalServerError;
                message = exception.Message;
                logger.LogError(exception, "Unhandled exception occurred");
                break;
        }

        context.Response.StatusCode = (int)status;

        var response = new RestErrorResponse(status, message, validationErrors);

        var json = JsonSerializer.Serialize(response);
        await context.Response.WriteAsync(json);
    }
}