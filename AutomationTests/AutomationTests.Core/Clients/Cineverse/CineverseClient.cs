using GraphQL.Client.Http;
using RestSharp;

namespace AutomationTests.Core.Clients.Cineverse;

public partial class CineverseClient(
    RestClient restClient,
    GraphQLHttpClient graphQlClient,
    string? token = null
);