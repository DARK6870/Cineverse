using AutomationTests.Core.Cineverse.Constants;
using AutomationTests.Core.Cineverse.DataGenerators;
using AutomationTests.Core.Common.Extensions;
using AutomationTests.Models.Cineverse.Requests.Screening;
using AutomationTests.Models.Generic;
using GraphQL.Client.Http;
using Xunit;

namespace AutomationTests.Core.Cineverse.Client;

public partial class CineverseClient
{
    public async Task<BaseResponse<string>> CreateScreening(string movieId, string hallId)
    {
        var createScreeningRequest = ScreeningDataGenerator.ValidCreateScreeningRequest(movieId, hallId);
        return await CreateScreening(createScreeningRequest);
    }
    
    public async Task<BaseResponse<string>> CreateScreening(CreateScreeningRequest createScreeningRequest)
    {
        var request = new GraphQLHttpRequest
        {
            Query = ScreeningConstants.CreateScreeningMutation,
            Variables = new { request = createScreeningRequest }
        };

        return await graphQlClient.SendAsync<string>(request, TestContext.Current.CancellationToken);
    }
    
    public async Task<BaseResponse<bool>> UpdateScreening(UpdateScreeningRequest updateScreeningRequest)
    {
        var request = new GraphQLHttpRequest
        {
            Query = ScreeningConstants.UpdateScreeningMutation,
            Variables = new { request = updateScreeningRequest }
        };

        return await graphQlClient.SendAsync<bool>(request, TestContext.Current.CancellationToken);
    }
    
    public async Task<BaseResponse<bool>> DeleteScreening(string id)
    {
        var request = new GraphQLHttpRequest
        {
            Query = ScreeningConstants.DeleteScreeningMutation,
            Variables = new { id }
        };

        return await graphQlClient.SendAsync<bool>(request, TestContext.Current.CancellationToken);
    }
}