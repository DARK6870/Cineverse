using System.Net.Http.Headers;
using AutomationTests.Core.Cineverse.Client;
using AutomationTests.Core.Common.Configuration;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.SystemTextJson;
using Infrastructure.Common.Json.Configuration;
using RestSharp;
using RestSharp.Authenticators;

namespace AutomationTests.Core.Cineverse.Helpers;

public static class CineverseClientFactory
{
    public static (CineverseClient client, RestClient rest, GraphQLHttpClient graphql) Create(string? token)
    {
        var restClientOptions = new RestClientOptions(TestConfiguration.Cineverse.HomepageUrl);
        var httpClient = new HttpClient { BaseAddress = new Uri(TestConfiguration.Cineverse.HomepageUrl) };

        if (!string.IsNullOrWhiteSpace(token))
        {
            restClientOptions.Authenticator = new JwtAuthenticator(token);
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }

        var restClient = new RestClient(restClientOptions);
        var graphQlClient = new GraphQLHttpClient(
            new GraphQLHttpClientOptions
            {
                EndPoint = new Uri($"{TestConfiguration.Cineverse.HomepageUrl}{TestConfiguration.Cineverse.CineverseApiBasePath}/graphql")
            },
            new SystemTextJsonSerializer(JsonSerializerConfiguration.GetDefault()),
            httpClient
        );

        return (new CineverseClient(restClient, graphQlClient), restClient, graphQlClient);
    }
}