using AutomationTests.Core.Common.Extensions;
using AutomationTests.Core.Identity.Constants;
using AutomationTests.Models.Generic;
using AutomationTests.Models.Identity.Requests;
using AutomationTests.Models.Identity.Responses;
using RestSharp;
using Xunit;

namespace AutomationTests.Core.Identity.Client;

public partial class IdentityClient
{
    public async Task<BaseResponse<AuthenticationResponse>> Login(LoginRequest loginRequest)
    {
        var request = new RestRequest
        {
            Resource = IdentityRoutes.Login,
            Method = Method.Post
        }.AddJsonBody(loginRequest);
        
        return await restClient.SendAsync<AuthenticationResponse>(request, TestContext.Current.CancellationToken);
    }
}