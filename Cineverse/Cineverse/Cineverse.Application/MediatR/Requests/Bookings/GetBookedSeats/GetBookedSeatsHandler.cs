using Cineverse.Mongo.Repositories.Booking;
using MediatR;
using MongoDB.Driver.Linq;

namespace Cineverse.Application.MediatR.Requests.Bookings.GetBookedSeats;

public class GetBookedSeatsHandler(
    IBookingRepository bookingRepository
) : IRequestHandler<GetBookedSeatsRequest, List<string>>
{
    public async Task<List<string>> Handle(GetBookedSeatsRequest request, CancellationToken cancellationToken)
    {
        return await bookingRepository.AsQueryable()
            .Where(x => x.ScreeningId == request.ScreeningId)
            .SelectMany(x => x.SeatIds)
            .Distinct()
            .ToListAsync(cancellationToken);
    }
}