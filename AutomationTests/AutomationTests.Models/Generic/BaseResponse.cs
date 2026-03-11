namespace AutomationTests.Models.Generic;

public record BaseResponse<T>
{
    public string? ErrorMessage { get; init; }
    public string[] ValidationErrors { get; init; } = [];
    public T? Data { get; init; }
    
    public bool HasValidationErrors => ValidationErrors.Length > 0;
    public bool HasError => ErrorMessage is not null;
    public bool IsSuccess => !HasError && !HasValidationErrors;

    public static BaseResponse<T> WithData(T data) => new() { Data = data };
    public static BaseResponse<T> WithError(string message) => new() { ErrorMessage = message };
    public static BaseResponse<T> WithValidationErrors(string[] errors) => new() { ValidationErrors = errors };
}