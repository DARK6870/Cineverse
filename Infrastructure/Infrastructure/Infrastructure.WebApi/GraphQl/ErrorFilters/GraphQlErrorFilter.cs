using FluentValidation;
using Infrastructure.Common.Exceptions.Base;
using Microsoft.Extensions.Logging;

namespace Infrastructure.WebApi.GraphQl.ErrorFilters;

public class GraphQlErrorFilter(
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
        
        var baseException = error.Exception?.GetBaseException();
        
        switch (baseException)
        {
            case BaseException exception:
                HandleApiErrorException(errorBuilder, exception);
                break;
            case ValidationException exception:
                HandleValidationException(errorBuilder, exception);
                break;
            default:
                logger.LogError("Unhandled error occured, error: {@error}", error.Exception);
                break;
        }
        
        return errorBuilder.Build();
    }

    private static void HandleApiErrorException(IErrorBuilder errorBuilder, BaseException exception)
    {
        errorBuilder
            .SetMessage(exception.ErrorMessage)
            .SetCode(((int)exception.StatusCode)
                .ToString());
    }
    
    private static void HandleValidationException(IErrorBuilder errorBuilder, ValidationException exception)
    {
        errorBuilder
            .SetMessage("Invalid request")
            .SetCode("INVALID_INPUT")
            .SetExtension("validationErrors", exception.Errors);
    }
}