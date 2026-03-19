using AutomationTests.Core.Identity.Constants;
using AutomationTests.Models.Identity.Requests;
using AutomationTests.Models.Identity.Responses;
using RestSharp;

namespace AutomationTests.Core.Identity.Client;

public class IdentityServiceClient(RestClient restClient)
{
    public Task<string> GetAdminToken() => GetToken("admin@cineverse.com", "TestPass_1");

    public Task<string> GetUserToken() => GetToken("user@cineverse.com", "TestPass_1");

    private async Task<string> GetToken(string email, string password)
    {
        var loginRequest = new LoginRequest(email, password);
        var response = await restClient.ExecuteAsync<AuthenticationResponse>(
            new RestRequest(IdentityRoutes.Login, Method.Post)
                .AddJsonBody(loginRequest)
        );

        return response.Data?.AccessToken
               ?? throw new InvalidOperationException("Unexpected error occurred while retrieving token");
    }
}