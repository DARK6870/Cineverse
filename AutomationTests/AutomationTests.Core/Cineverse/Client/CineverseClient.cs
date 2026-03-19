using GraphQL.Client.Http;
using RestSharp;

namespace AutomationTests.Core.Cineverse.Client;

public partial class CineverseClient(
    RestClient restClient,
    GraphQLHttpClient graphQlClient
);