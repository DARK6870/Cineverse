using System.Net;
using System.Text.Json;
using AutomationTests.Models.Generic;
using GraphQL;
using GraphQL.Client.Http;
using Infrastructure.Common.Helpers;

namespace AutomationTests.Core.Common.Extensions;

public static class GraphQlHttpClientExtensions
{
    public static async Task<BaseResponse<T>> SendAsync<T>(
        this GraphQLHttpClient client,
        GraphQLRequest request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var response = await client.SendQueryAsync<JsonDocument>(request, cancellationToken);

            if (response.Errors?.Length > 0)
            {
                var firstError = response.Errors.First();
                var statusCode = ExtractStatusCode(firstError.Extensions);

                if (firstError.Extensions?.TryGetValue("validationErrors", out var errors) == true
                    && errors is IEnumerable<object> validationErrors)
                {
                    return new BaseResponse<T>
                    {
                        StatusCode = statusCode,
                        ErrorMessage = firstError.Message,
                        ValidationErrors = validationErrors.Select(e => e.ToString()!).ToArray()
                    };
                }

                return new BaseResponse<T>
                {
                    StatusCode = statusCode,
                    ErrorMessage = firstError.Message
                };
            }

            var root = response.Data.RootElement;
            var data = JsonHelper.Deserialize<T>(root.EnumerateObject().First().Value.GetRawText());
            return new BaseResponse<T>
            {
                StatusCode = HttpStatusCode.OK,
                Data = data
            };
        }
        catch (GraphQLHttpRequestException ex)
        {
            var response = JsonHelper.Deserialize<GraphQLResponse<JsonDocument>>(ex.Content!);
            var message = response.Errors?.First().Message ?? ex.Message;
            var statusCode = ExtractStatusCode(response.Errors?.First()?.Extensions);
            return new BaseResponse<T>
            {
                StatusCode = statusCode,
                ErrorMessage = message
            };
        }
    }

    private static HttpStatusCode ExtractStatusCode(
        IDictionary<string, object?>? extensions,
        HttpStatusCode fallback = HttpStatusCode.BadRequest
    )
    {
        if (extensions?.TryGetValue("statusCode", out var raw) == true)
        {
            return raw switch
            {
                HttpStatusCode code => code,
                int intCode => (HttpStatusCode)intCode,
                string strCode when int.TryParse(strCode, out var parsed) => (HttpStatusCode)parsed,
                _ => fallback
            };
        }

        return fallback;
    }
}