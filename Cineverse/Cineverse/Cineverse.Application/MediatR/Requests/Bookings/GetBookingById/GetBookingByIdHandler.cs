using System.Net;
using Auth.Models.Enums;
using Cineverse.Mongo.Repositories.Booking;
using Cineverse.Mongo.Schemas.Entities;
using Infrastructure.Context.UserContext;
using Infrastructure.WebApi.Exceptions;
using MediatR;

namespace Cineverse.Application.MediatR.Requests.Bookings.GetBookingById;

public class GetBookingByIdHandler(
    IBookingRepository bookingRepository,
    IUserContext userContext
) : IRequestHandler<GetBookingByIdRequest, BookingEntity?>
{
    public async Task<BookingEntity?> Handle(GetBookingByIdRequest request, CancellationToken cancellationToken)
    {
        var booking = await bookingRepository.FindByIdAsync(request.Id, cancellationToken);
        
        if (userContext.Role == Role.User && booking?.UserId != userContext.UserId)
            throw new ApiRequestException("You don't have permissions to get this booking", HttpStatusCode.Forbidden);
        
        return booking;
    }
}