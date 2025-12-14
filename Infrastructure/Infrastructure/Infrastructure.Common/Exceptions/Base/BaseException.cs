using System.Net;

namespace Infrastructure.Common.Exceptions.Base;

public class BaseException(string error, HttpStatusCode statusCode) : Exception(error)
{
    public string ErrorMessage { get; } = error;
    
    public HttpStatusCode StatusCode { get; } = statusCode;
}