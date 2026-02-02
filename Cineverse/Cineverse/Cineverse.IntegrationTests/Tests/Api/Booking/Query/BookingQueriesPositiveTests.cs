using Cineverse.IntegrationTests.Core.Categories;
using Cineverse.IntegrationTests.Core.Collections;
using Cineverse.IntegrationTests.Core.Factory;
using Cineverse.IntegrationTests.Shared.Constants.GraphQl;
using Cineverse.IntegrationTests.Shared.Extensions;
using Cineverse.IntegrationTests.Shared.Helpers;
using Cineverse.Mongo.Schemas.Entities;
using GraphQL.Client.Http;

namespace Cineverse.IntegrationTests.Tests.Api.Booking.Query;

[Collection(nameof(MainCollection))]
[Trait("Category", Categories.IntegrationTests)]
public class BookingQueriesPositiveTests(CineverseWebApplicationFactory factory)
{
    private readonly GraphQLHttpClient _client = factory.CreateGraphQlHttpClient();
    
    [Fact]
    public async Task GetBookingById_ShouldReturnBooking_WhenBookingBelongsToCurrentUser()
    {
        // Arrange
        var booking = await factory.GenerateBookingAsync();
        var request = new GraphQLHttpRequest
        {
            Query = BookingGraphQlConstants.GetBookingByIdQuery,
            Variables = new { id = booking.Id }
        };

        // Act
        var bookingById = await _client.SendQueryAndExtractDataAsync<BookingEntity>(request, TestContext.Current.CancellationToken);
        
        // Assert
        Assert.EquivalentWithExclusions(
            booking,
            bookingById,
            x => x.DateCreated
        );
    }
}