using AutomationTests.Core.Common.Configuration;
using AutomationTests.Core.Identity.Client;
using RestSharp;
using RestSharp.Authenticators;

namespace AutomationTests.Core.Identity.Helpers;

public static class IdentityClientFactory
{
    public static (IdentityClient client, RestClient rest) Create(string? token)
    {
        var restClientOptions = new RestClientOptions(TestConfiguration.Cineverse.HomepageUrl + TestConfiguration.Cineverse.IdentityApiBasePath);

        if (!string.IsNullOrWhiteSpace(token))
            restClientOptions.Authenticator = new JwtAuthenticator(token);

        var restClient = new RestClient(restClientOptions);

        return (new IdentityClient(restClient), restClient);
    }
}