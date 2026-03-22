using AutomationTests.Core.Cineverse.Helpers;
using AutomationTests.Core.Common.Helpers;
using AutomationTests.Models.Cineverse.Entities;
using GraphQL.Client.Http;
using RestSharp;
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
        (AdminClient, UserClient, UnauthorizedClient) = 
            await CineverseFixtureSetupHelper.CreateClientsAsync(_restClients, _graphQlClients);
        
        DefaultHall = await CineverseFixtureSetupHelper.CreateDefaultHallAsync(AdminClient);
    }

    public async ValueTask DisposeAsync()
    {
        await DatabaseCleanUpHelper.CleanDataFromDatabase(TestsStartDate);
        foreach (var client in _restClients) client.Dispose();
        foreach (var client in _graphQlClients) client.Dispose();
    }
}