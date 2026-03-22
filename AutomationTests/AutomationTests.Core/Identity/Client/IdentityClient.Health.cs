using RestSharp;

namespace AutomationTests.Core.Identity.Client;

public partial class IdentityClient
{
    public async Task<bool> IsHealthy()
    {
        var request = new RestRequest
        {
            Method = Method.Get,
            Resource = "/health"
        };
        
        var result= await restClient.ExecuteAsync(request);

        return result.IsSuccessful;
    }
}