using AutomationTests.Core.Common.Configuration;
using AutomationTests.Core.Identity.Client;
using RestSharp;

namespace AutomationTests.Core.Identity.Helpers;

public static class IdentityFixtureSetupHelper
{
    public static async Task<(
        IdentityClient Admin,
        IdentityClient User,
        IdentityClient Unauthorized
    )> CreateClientsAsync(List<RestClient> restClients)
    {
        var identityClient = new IdentityClient(
            new RestClient(TestConfiguration.Cineverse.HomepageUrl + TestConfiguration.Cineverse.IdentityApiBasePath)
        );

        var adminToken = await identityClient.GetAdminToken();
        var userToken = await identityClient.GetUserToken();

        var (adminClient, adminRest) = IdentityClientFactory.Create(adminToken);
        var (userClient, userRest) = IdentityClientFactory.Create(userToken);
        var (unauthorizedClient, unauthRest) = IdentityClientFactory.Create(null);

        restClients.AddRange([adminRest, userRest, unauthRest]);

        return (adminClient, userClient, unauthorizedClient);
    }
}