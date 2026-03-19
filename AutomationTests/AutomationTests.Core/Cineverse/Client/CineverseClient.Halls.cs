using AutomationTests.Core.Cineverse.Constants;
using AutomationTests.Core.Common.Extensions;
using AutomationTests.Models.Cineverse.Entities;
using AutomationTests.Models.Cineverse.Requests.Hall;
using AutomationTests.Models.Generic;
using GraphQL.Client.Http;
using Xunit;

namespace AutomationTests.Core.Cineverse.Client;

public partial class CineverseClient
{
    public async Task<BaseResponse<HallEntity[]>> GetHalls()
    {
        var request = new GraphQLHttpRequest
        {
            Query = HallConstants.GetHallsQuery
        };

        return await graphQlClient.SendAsync<HallEntity[]>(request, TestContext.Current.CancellationToken);
    }
    
    public async Task<BaseResponse<HallEntity>> GetHallById(string id)
    {
        var request = new GraphQLHttpRequest
        {
            Query = HallConstants.GetHallByIdQuery,
            Variables = new { id }
        };

        return await graphQlClient.SendAsync<HallEntity>(request, TestContext.Current.CancellationToken);
    }
    
    public async Task<BaseResponse<string>> CreateHall(CreateHallRequest createHallRequest)
    {
        var request = new GraphQLHttpRequest
        {
            Query = HallConstants.CreateHallMutation,
            Variables = new { request = createHallRequest }
        };
        
        return await graphQlClient.SendAsync<string>(request, TestContext.Current.CancellationToken);
    }

    public async Task<BaseResponse<bool>> UpdateHall(UpdateHallRequest updateHallRequest)
    {
        var request = new GraphQLHttpRequest
        {
            Query = HallConstants.UpdateHallMutation,
            Variables = new { request = updateHallRequest }
        };
        
        return await graphQlClient.SendAsync<bool>(request, TestContext.Current.CancellationToken);
    }
    
    public async Task<BaseResponse<bool>> DeleteHall(string id)
    {
        var request = new GraphQLHttpRequest
        {
            Query = HallConstants.DeleteHallMutation,
            Variables = new { id }
        };
        
        return await graphQlClient.SendAsync<bool>(request, TestContext.Current.CancellationToken);
    }
}