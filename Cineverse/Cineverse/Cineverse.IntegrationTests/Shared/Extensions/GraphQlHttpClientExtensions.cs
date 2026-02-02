using System.Text.Json;
using GraphQL;
using GraphQL.Client.Http;
using HotChocolate;
using Infrastructure.Common.Helpers;

namespace Cineverse.IntegrationTests.Shared.Extensions;

public static class GraphQlHttpClientExtensions
{
    public static async Task<string> SendQueryAndExtractErrorAsync(
        this GraphQLHttpClient client,
        GraphQLRequest request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var response = await client.SendQueryAsync<JsonDocument>(request, cancellationToken);
            return response.Errors != null ? response.Errors.First().Message : string.Empty;
        }
        catch (GraphQLHttpRequestException ex)
        {
            var response = JsonHelper.Deserialize<GraphQLResponse<JsonDocument>>(ex.Content!);
            return response.Errors?.First().Message ?? ex.Message;
        }
    }

    public static async Task<T> SendQueryAndExtractDataAsync<T>(
        this GraphQLHttpClient client,
        GraphQLRequest request,
        CancellationToken cancellationToken
    )
    {
        var response = await client.SendQueryAsync<JsonDocument>(request, cancellationToken);
        if (response.Errors?.First() != null)
            throw new GraphQLException(response.Errors.First().Message);
        
        var root = response.Data.RootElement;
        return JsonHelper.Deserialize<T>(root.EnumerateObject().First().Value.GetRawText());
    }

    public static async Task<string> SendMutationAndExtractErrorAsync(
        this GraphQLHttpClient client,
        GraphQLRequest request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var response = await client.SendMutationAsync<JsonDocument>(request, cancellationToken);
            return response.Errors != null ? response.Errors.First().Message : string.Empty;
        }
        catch (GraphQLHttpRequestException ex)
        {
            var response = JsonHelper.Deserialize<GraphQLResponse<JsonDocument>>(ex.Content!);
            return response.Errors?.First().Message ?? ex.Message;
        }
    }

    public static async Task<string[]> SendMutationAndExtractValidationErrorsAsync(
        this GraphQLHttpClient client,
        GraphQLRequest request,
        CancellationToken cancellationToken
    )
    {
        var response = await client.SendMutationAsync<JsonDocument>(request, cancellationToken);
        if (response.Errors != null && response.Errors.First().Extensions!.TryGetValue("validationErrors", out var errors))
        {
            if (errors is IEnumerable<object> listObj)
            {
                var errorsArray = listObj.Select(e => e.ToString()!).ToArray();
                return errorsArray;
            }
        }
        
        return [];
    }
}