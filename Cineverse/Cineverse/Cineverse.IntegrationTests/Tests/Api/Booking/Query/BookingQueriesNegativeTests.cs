using Cineverse.IntegrationTests.Core.Categories;
using Cineverse.IntegrationTests.Core.Collections;
using Cineverse.IntegrationTests.Core.Factory;
using Cineverse.IntegrationTests.Shared.Constants.GraphQl;
using Cineverse.IntegrationTests.Shared.Extensions;
using Cineverse.IntegrationTests.Shared.Helpers;
using GraphQL;
using GraphQL.Client.Http;
using MongoDB.Bson;

namespace Cineverse.IntegrationTests.Tests.Api.Booking.Query;

[Collection(nameof(MainCollection))]
[Trait("Category", Categories.IntegrationTests)]
public class BookingQueriesNegativeTests(CineverseWebApplicationFactory factory)
{
    private readonly GraphQLHttpClient _client = factory.CreateGraphQlHttpClient();
    
    [Fact]
    public async Task GetBookingById_ShouldReturnError_WhenBookingBelongsToAnotherUser()
    {
        // Arrange
        var booking = await factory.GenerateBookingAsync(ObjectId.GenerateNewId().ToString());

        var request = new GraphQLRequest
        {
            Query = BookingGraphQlConstants.GetBookingByIdQuery,
            Variables = new
            {
                id = booking.Id
            }
        };
        
        // Act
        var error = await _client.SendQueryAndExtractErrorAsync(request, TestContext.Current.CancellationToken);
        
        // Assert
        Assert.NotEmpty(error);
        Assert.Equal("You don't have permissions to get this booking", error);
    }

    [Fact]
    public async Task GetBookingById_ShouldReturnError_WhenBookingDoesNotExists()
    {
        // Arrange
        var request = new GraphQLRequest
        {
            Query = BookingGraphQlConstants.GetBookingByIdQuery,
            Variables = new
            {
                id = ObjectId.GenerateNewId().ToString()
            }
        };
        
        // Act
        var error = await _client.SendQueryAndExtractErrorAsync(request, TestContext.Current.CancellationToken);
        
        // Assert
        Assert.NotEmpty(error);
        Assert.Equal("Booking was not found", error);
    }
}