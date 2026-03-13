using Cineverse.Mongo.Repositories.Booking;
using Cineverse.Mongo.Schemas.Entities;
using Infrastructure.Context.UserContext;
using MediatR;

namespace Cineverse.Application.MediatR.Requests.Bookings.GetUserBookings;

public class GetUserBookingsHandler(
    IUserContext userContext,
    IBookingRepository bookingRepository
) : IRequestHandler<GetUserBookingsRequest, IQueryable<BookingEntity>>
{
    public Task<IQueryable<BookingEntity>> Handle(GetUserBookingsRequest request, CancellationToken cancellationToken)
    {
        return Task.FromResult(bookingRepository.AsQueryable().Where(x => x.UserId == userContext.UserId)); 
    }
}