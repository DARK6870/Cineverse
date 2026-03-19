using AutomationTests.Core.Cineverse.Constants;
using AutomationTests.Core.Common.Extensions;
using AutomationTests.Models.Cineverse.Entities;
using AutomationTests.Models.Cineverse.Requests.Booking;
using AutomationTests.Models.Generic;
using GraphQL.Client.Http;
using Xunit;

namespace AutomationTests.Core.Cineverse.Client;

public partial class CineverseClient
{
    public async Task<BaseResponse<BookingEntity[]>> GetBookings()
    {
        var request = new GraphQLHttpRequest
        {
            Query = BookingConstants.GetBookingsQuery
        };

        return await graphQlClient.SendAsync<BookingEntity[]>(request, TestContext.Current.CancellationToken);
    }
    
    public async Task<BaseResponse<BookingEntity[]>> GetUserBookings()
    {
        var request = new GraphQLHttpRequest
        {
            Query = BookingConstants.GetUserBookingsQuery
        };

        return await graphQlClient.SendAsync<BookingEntity[]>(request, TestContext.Current.CancellationToken);
    }
    
    public async Task<BaseResponse<BookingEntity>> GetBookingById(string id)
    {
        var request = new GraphQLHttpRequest
        {
            Query = BookingConstants.GetBookingByIdQuery,
            Variables = new { id }
        };

        return await graphQlClient.SendAsync<BookingEntity>(request, TestContext.Current.CancellationToken);
    }
    
    public async Task<BaseResponse<string>> CreateBooking(CreateBookingRequest createBookingRequest)
    {
        var request = new GraphQLHttpRequest
        {
            Query = BookingConstants.CreateBookingMutation,
            Variables = new { request = createBookingRequest }
        };

        return await graphQlClient.SendAsync<string>(request, TestContext.Current.CancellationToken);
    }
}