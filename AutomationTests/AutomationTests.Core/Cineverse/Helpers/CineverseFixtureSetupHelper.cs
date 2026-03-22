using AutomationTests.Core.Cineverse.Client;
using AutomationTests.Core.Cineverse.DataGenerators;
using AutomationTests.Core.Common.Configuration;
using AutomationTests.Core.Identity.Client;
using AutomationTests.Models.Cineverse.Entities;
using GraphQL.Client.Http;
using RestSharp;

namespace AutomationTests.Core.Cineverse.Helpers;

public static class CineverseFixtureSetupHelper
{
    public static async Task<(CineverseClient Admin, CineverseClient User, CineverseClient Unauthorized)> CreateClientsAsync(
        List<RestClient> restClients,
        List<GraphQLHttpClient> graphQlClients
    )
    {
        var identityClient = new IdentityClient(
            new RestClient(TestConfiguration.Cineverse.HomepageUrl + TestConfiguration.Cineverse.IdentityApiBasePath)
        );

        var adminToken = await identityClient.GetAdminToken();
        var userToken = await identityClient.GetUserToken();

        var (adminClient, adminRest, adminGql) = CineverseClientFactory.Create(adminToken);
        var (userClient, userRest, userGql) = CineverseClientFactory.Create(userToken);
        var (unauthorizedClient, unauthRest, unauthGql) = CineverseClientFactory.Create(null);

        restClients.AddRange([adminRest, userRest, unauthRest]);
        graphQlClients.AddRange([adminGql, userGql, unauthGql]);

        return (adminClient, userClient, unauthorizedClient);
    }

    public static async Task<HallEntity> CreateDefaultHallAsync(CineverseClient adminClient)
    {
        var createHallResponse = await adminClient.CreateHall(HallDataGenerator.DefaultHall);
        var hallId = createHallResponse.Data ?? throw new NullReferenceException("Unable to create a default hall");

        var getHallByIdResponse = await adminClient.GetHallById(hallId);
        return getHallByIdResponse.Data ?? throw new NullReferenceException("Unable to retrieve a default hall");
    }
}