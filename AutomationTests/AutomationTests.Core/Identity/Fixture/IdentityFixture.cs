using AutomationTests.Core.Common.Helpers;
using AutomationTests.Core.Identity.Client;
using AutomationTests.Core.Identity.Helpers;
using RestSharp;
using Xunit;

namespace AutomationTests.Core.Identity.Fixture;

public sealed class IdentityFixture : IAsyncLifetime
{
    private readonly List<RestClient> _restClients = new();

    public IdentityClient AdminClient { get; private set; } = null!;
    public IdentityClient UserClient { get; private set; } = null!;
    public IdentityClient UnauthorizedClient { get; private set; } = null!;
    private DateTime TestsStartDate { get; init; } = DateTime.UtcNow;

    public async ValueTask InitializeAsync()
    {
        (AdminClient, UserClient, UnauthorizedClient) = await IdentityFixtureSetupHelper.CreateClientsAsync(_restClients);
    }

    public async ValueTask DisposeAsync()
    {
        await DatabaseCleanUpHelper.CleanDataFromDatabase(TestsStartDate);
        foreach (var client in _restClients) client.Dispose();
    }
}