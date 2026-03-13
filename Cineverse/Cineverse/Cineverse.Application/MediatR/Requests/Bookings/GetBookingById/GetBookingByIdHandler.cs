using Auth.Models.Enums;
using Cineverse.Mongo.Repositories.Booking;
using Cineverse.Mongo.Schemas.Entities;
using Infrastructure.Common.Exceptions;
using Infrastructure.Context.UserContext;
using MediatR;

namespace Cineverse.Application.MediatR.Requests.Bookings.GetBookingById;

public class GetBookingByIdHandler(
    IBookingRepository bookingRepository,
    IUserContext userContext
) : IRequestHandler<GetBookingByIdRequest, BookingEntity>
{
    public async Task<BookingEntity> Handle(GetBookingByIdRequest request, CancellationToken cancellationToken)
    {
        var booking = await bookingRepository.FindByIdOrThrowAsync(request.Id, cancellationToken);
        
        if (userContext.Role == Role.User && booking?.UserId != userContext.UserId)
            throw new ForbiddenException("You don't have permissions to get this booking");
        
        return booking;
    }
}