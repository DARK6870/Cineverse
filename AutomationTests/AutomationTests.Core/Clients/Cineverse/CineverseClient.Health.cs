using RestSharp;

namespace AutomationTests.Core.Clients.Cineverse;

public partial class CineverseClient
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