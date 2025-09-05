using System.Net;

namespace Cineverse.Domain.Common.Exceptions;

public class ApiRequestException : Exception
{
    public List<string> Errors { get; }
    public HttpStatusCode StatusCode { get; }
    

    public ApiRequestException(List<string> errors, HttpStatusCode statusCode) :base(string.Join("\n", errors))
    {
        Errors = errors ?? throw new ArgumentNullException(nameof(errors));
        StatusCode = statusCode;
    }
    
    public ApiRequestException(string error, HttpStatusCode statusCode)
        : base(error)
    {
        Errors = [error ?? throw new ArgumentNullException(nameof(error))];
        StatusCode = statusCode;
    }
}