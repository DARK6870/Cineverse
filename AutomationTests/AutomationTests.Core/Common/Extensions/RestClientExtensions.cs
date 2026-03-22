using System.Text.Json;
using AutomationTests.Models.Generic;
using RestSharp;

namespace AutomationTests.Core.Common.Extensions;

public static class RestClientExtensions
{
    public static async Task<BaseResponse<T>> SendAsync<T>(
        this RestClient client,
        RestRequest request,
        CancellationToken cancellationToken
    ) where T : notnull
    {
        var response = await client.ExecuteAsync<T>(request, cancellationToken);

        if (response is { IsSuccessful: false, Content: not null })
        {
            var errorResponse = JsonSerializer.Deserialize<RestErrorResponse>(response.Content)
                                ?? throw new InvalidOperationException("Unable to deserialize response");

            return new BaseResponse<T>
            {
                StatusCode = response.StatusCode,
                ErrorMessage = errorResponse.ErrorMessage,
                ValidationErrors = errorResponse.ValidationErrors
            };
        }

        return new BaseResponse<T>
        {
            Data =  response.Data
        };
    }
}