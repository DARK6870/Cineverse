using System.Net;

namespace AutomationTests.Models.Generic;

public record BaseResponse<T>
{
    public HttpStatusCode StatusCode { get; init; } = HttpStatusCode.OK;
    public string? ErrorMessage { get; init; }
    public string[] ValidationErrors { get; init; } = [];
    public T? Data { get; init; }
    
    public bool IsSuccess => ErrorMessage is null && ValidationErrors.Length is 0;
}