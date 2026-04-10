using GraphQL.Client.Http;
using RestSharp;

namespace AutomationTests.Core.Cineverse.Api.Client;

public partial class CineverseClient(
    RestClient restClient,
    GraphQLHttpClient graphQlClient
);