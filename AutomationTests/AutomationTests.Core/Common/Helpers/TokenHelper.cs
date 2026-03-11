using AutomationTests.Core.Common.Constants.Identity;
using AutomationTests.Core.Configuration;
using AutomationTests.Models.Identity.Requests;
using AutomationTests.Models.Identity.Responses;
using RestSharp;

namespace AutomationTests.Core.Common.Helpers;

public static class TokenHelper
{
    private static readonly Uri IdentityBaseAddress = new(TestConfiguration.Cineverse.HomepageUrl + TestConfiguration.Cineverse.IdentityApiBasePath);

    private static readonly HttpClient HttpClient = new()
    {
        BaseAddress = IdentityBaseAddress
    };

    private static readonly RestClient RestClient = new(HttpClient);

    public static Task<string> GetAdminToken() => GetToken("admin@cineverse.com", "TestPass_1");

    public static Task<string> GetUserToken() => GetToken("user@cineverse.com", "TestPass_1");

    private static async Task<string> GetToken(string email, string password)
    {
        var loginRequest = new LoginRequest(email, password);
        var response = await RestClient.ExecuteAsync<AuthenticationResponse>(
            new RestRequest(IdentityRoutes.Login, Method.Post)
                .AddJsonBody(loginRequest)
        );

        return response.Data?.AccessToken
               ?? throw new InvalidOperationException("Unexpected error occurred while retrieving token");
    }
}
