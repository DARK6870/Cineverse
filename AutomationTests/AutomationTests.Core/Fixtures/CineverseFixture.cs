using AutomationTests.Core.Clients.Cineverse;
using AutomationTests.Core.Clients.IdentityService;
using AutomationTests.Core.Common.Helpers;
using AutomationTests.Core.Configuration;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.SystemTextJson;
using Infrastructure.Common.Json.Configuration;
using RestSharp;
using RestSharp.Authenticators;
using Xunit;

namespace AutomationTests.Core.Fixtures;

public class CineverseFixture : IAsyncLifetime
{
    private readonly List<RestClient> _restClients = new();
    private readonly List<GraphQLHttpClient> _graphQlClients = new();

    public CineverseClient AdminClient { get; private set; } = null!;
    public CineverseClient UserClient { get; private set; } = null!;
    public CineverseClient UnauthorizedClient { get; private set; } = null!;
    public DateTime TestsStartDate { get; init; } = DateTime.UtcNow;

    public async ValueTask InitializeAsync()
    {
        var identityClient = new IdentityServiceClient(
            new RestClient(TestConfiguration.Cineverse.HomepageUrl + TestConfiguration.Cineverse.IdentityApiBasePath));

        var adminToken = await identityClient.GetAdminToken();
        var userToken = await identityClient.GetUserToken();

        AdminClient = CreateClient(adminToken);
        UserClient = CreateClient(userToken);
        UnauthorizedClient = CreateClient(null);
    }

    private CineverseClient CreateClient(string? token)
    {
        var restClientOptions = new RestClientOptions(TestConfiguration.Cineverse.HomepageUrl);
        var httpClient = new HttpClient { BaseAddress = new Uri(TestConfiguration.Cineverse.HomepageUrl) };
        
        if (!string.IsNullOrWhiteSpace(token))
        {
            restClientOptions.Authenticator = new JwtAuthenticator(token);
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
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

        _restClients.Add(restClient);
        _graphQlClients.Add(graphQlClient);

        return new CineverseClient(restClient, graphQlClient);
    }

    public async ValueTask DisposeAsync()
    {
        await DatabaseCleanUpHelper.CleanDataFromDatabase(TestsStartDate);
        
        foreach (var client in _restClients) client.Dispose();
        foreach (var client in _graphQlClients) client.Dispose();
    }
}