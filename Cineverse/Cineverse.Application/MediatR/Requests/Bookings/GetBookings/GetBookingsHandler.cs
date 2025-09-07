using Cineverse.Mongo.Repositories.Booking;
using Cineverse.Mongo.Schemas.Entities;
using MediatR;

namespace Cineverse.Application.MediatR.Requests.Bookings.GetBookings;

public class GetBookingsHandler(
    IBookingRepository bookingRepository
) : IRequestHandler<GetBookingsRequest, IQueryable<BookingEntity>>
{
    public Task<IQueryable<BookingEntity>> Handle(GetBookingsRequest request, CancellationToken cancellationToken)
    {
        return Task.FromResult(bookingRepository.AsQueryable());
    }
}