using System.Net;

namespace Cineverse.Domain.Common.Exceptions;

public class ApiRequestException(string error, HttpStatusCode statusCode) : Exception(error)
{
    public string ErrorMessage { get; } = error;
    public HttpStatusCode StatusCode { get; } = statusCode;
}