using System.Net;
using Cineverse.Domain.Common.Exceptions;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace Cineverse.Infrastructure.GraphQl;

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
        
        switch (error.Exception?.GetBaseException())
        {
            case ApiRequestException apiRequestException:
                HandleApiErrorException(errorBuilder, apiRequestException);
                break;
            case ValidationException validationException:
                HandleValidationException(errorBuilder, validationException);
                break;
            //case EntityNotFoundException entityNotFoundException:
                //HandleEntityNotFoundException(errorBuilder, entityNotFoundException);
                //break;
            default:
                logger.LogError("Unhandled error occured, error: {@error}", error.Exception);
                break;
        }
        
        return errorBuilder.Build();
    }

    private static void HandleApiErrorException(IErrorBuilder errorBuilder, ApiRequestException exception)
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
    
    /*private static void HandleEntityNotFoundException(IErrorBuilder errorBuilder, EntityNotFoundException exception)
    {
        errorBuilder
            .SetMessage(exception.ErrorMessage)
            .SetCode(((int)HttpStatusCode.NotFound).ToString());
    }*/
}