using AutomationTests.Core.Common.Helpers;
using AutomationTests.Core.Configuration;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.SystemTextJson;
using Infrastructure.Common.Json.Configuration;
using RestSharp;
using Xunit;

namespace AutomationTests.Core.Fixtures;

public class CineverseFixture : IAsyncLifetime
{
    public RestClient RestClient { get; private set; } = null!;
    public GraphQLHttpClient GraphQlClient { get; private set; } = null!;
    public string AdminToken { get; private set; } = null!;
    public string UserToken { get; private set; } = null!;

    public async ValueTask InitializeAsync()
    {
        RestClient = new RestClient(TestConfiguration.Cineverse.HomepageUrl);

        GraphQlClient = new GraphQLHttpClient(
            new GraphQLHttpClientOptions
            {
                EndPoint = new Uri($"{TestConfiguration.Cineverse.HomepageUrl}{TestConfiguration.Cineverse.CineverseApiBasePath}" + "/graphql")
            },
            new SystemTextJsonSerializer(JsonSerializerConfiguration.GetDefault())
        );

        AdminToken = await TokenHelper.GetAdminToken();
        UserToken = await TokenHelper.GetUserToken();
    }

    public ValueTask DisposeAsync()
    {
        RestClient.Dispose();
        GraphQlClient.Dispose();
        
        return ValueTask.CompletedTask;
    }
}