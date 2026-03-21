using AutomationTests.Core.Cineverse.DataGenerators;
using AutomationTests.Core.Common.Configuration;
using AutomationTests.Core.Common.Helpers;
using AutomationTests.Core.Identity.Client;
using AutomationTests.Models.Cineverse.Entities;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.SystemTextJson;
using Infrastructure.Common.Json.Configuration;
using RestSharp;
using RestSharp.Authenticators;
using Xunit;
using CineverseClient = AutomationTests.Core.Cineverse.Client.CineverseClient;

namespace AutomationTests.Core.Cineverse.Fixture;

public sealed class CineverseFixture : IAsyncLifetime
{
    private readonly List<RestClient> _restClients = new();
    private readonly List<GraphQLHttpClient> _graphQlClients = new();

    public CineverseClient AdminClient { get; private set; } = null!;
    public CineverseClient UserClient { get; private set; } = null!;
    public CineverseClient UnauthorizedClient { get; private set; } = null!;
    public HallEntity DefaultHall { get; private set; } = null!;
    private DateTime TestsStartDate { get; init; } = DateTime.UtcNow;

    public async ValueTask InitializeAsync()
    {
        // TODO: move to extension methods or setup helper
        var identityClient = new IdentityServiceClient(new RestClient(TestConfiguration.Cineverse.HomepageUrl + TestConfiguration.Cineverse.IdentityApiBasePath));

        var adminToken = await identityClient.GetAdminToken();
        var userToken = await identityClient.GetUserToken();

        AdminClient = CreateClient(adminToken);
        UserClient = CreateClient(userToken);
        UnauthorizedClient = CreateClient(null);

        var createHallResponse = await AdminClient.CreateHall(HallDataGenerator.DefaultHall);
        var hallId = createHallResponse.Data ?? throw new NullReferenceException("Unable to create a default hall");
        var getHallByIdResponse = await AdminClient.GetHallById(hallId);
        DefaultHall = getHallByIdResponse.Data ?? throw new NullReferenceException("Unable to retrieve a default hall");
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