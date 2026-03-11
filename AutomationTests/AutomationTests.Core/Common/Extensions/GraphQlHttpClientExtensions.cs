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

                if (firstError.Extensions?.TryGetValue("validationErrors", out var errors) == true
                    && errors is IEnumerable<object> validationErrors)
                {
                    return BaseResponse<T>.WithValidationErrors(
                        validationErrors.Select(e => e.ToString()!).ToArray()
                    );
                }

                return BaseResponse<T>.WithError(firstError.Message);
            }

            var root = response.Data.RootElement;
            var data = JsonHelper.Deserialize<T>(root.EnumerateObject().First().Value.GetRawText());
            return BaseResponse<T>.WithData(data);
        }
        catch (GraphQLHttpRequestException ex)
        {
            var response = JsonHelper.Deserialize<GraphQLResponse<JsonDocument>>(ex.Content!);
            var message = response.Errors?.First().Message ?? ex.Message;
            return BaseResponse<T>.WithError(message);
        }
    }
}