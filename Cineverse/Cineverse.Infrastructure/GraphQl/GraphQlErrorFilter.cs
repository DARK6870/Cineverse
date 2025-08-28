using Cineverse.Infrastructure.Common.Exceptions;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace Cineverse.Infrastructure.GraphQl;

internal class GraphQlErrorFilter(
    ILogger<GraphQlErrorFilter> logger
) : IErrorFilter
{
    public IError OnError(IError error)
    {
        return GetErrorResponse(error);
    }

    private IError GetErrorResponse(IError error)
    {
        var errorBuilder = ErrorBuilder
            .FromError(error)
            .ClearExtensions()
            .ClearLocations();
        
        switch (error.Exception?.GetBaseException())
        {
            case ApiRequestException ApiRequestException:
                HandleApiErrorException(errorBuilder, ApiRequestException);
                break;
            case ValidationException validationException:
                HandleValidationException(errorBuilder, validationException);
                break;
            default:
                logger.LogError("Unhandled error occured, error: {@error}", error.Exception);
                break;
        }
        
        return errorBuilder.Build();
    }

    private static void HandleApiErrorException(IErrorBuilder errorBuilder, ApiRequestException ApiRequestException)
    {
        errorBuilder
            .SetMessage(ApiRequestException.Message)
            .SetCode(((int)ApiRequestException.StatusCode)
                .ToString());
    }
    
    private static void HandleValidationException(IErrorBuilder errorBuilder, ValidationException validationException)
    {
        errorBuilder
            .SetMessage("Invalid request")
            .SetCode("INVALID_INPUT")
            .SetExtension("validationErrors", validationException.Errors);
    }
}