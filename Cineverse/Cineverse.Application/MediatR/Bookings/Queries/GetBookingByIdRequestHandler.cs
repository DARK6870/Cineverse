using System.Net;
using Cineverse.Domain.Common.Exceptions;
using Cineverse.Identity.Services.UserContext;
using Cineverse.Mongo.Repositories.Booking;
using Cineverse.Mongo.Schemas.Entities;
using Cineverse.Mongo.Schemas.Enums;
using MediatR;

namespace Cineverse.Application.MediatR.Bookings.Queries;

public record GetBookingByIdRequest(string Id) : IRequest<BookingEntity?>;

public class GetBookingByIdRequestHandler(
    IBookingRepository bookingRepository,
    IUserContext userContext
) : IRequestHandler<GetBookingByIdRequest, BookingEntity?>
{
    public async Task<BookingEntity?> Handle(GetBookingByIdRequest request, CancellationToken cancellationToken)
    {
        var booking = await bookingRepository.FindByIdAsync(request.Id, cancellationToken);
        
        if (userContext.Role == Role.User && booking?.UserId != userContext.UserId)
            throw new ApiRequestException("Access denied", HttpStatusCode.Forbidden);
        
        return booking;
    }
}