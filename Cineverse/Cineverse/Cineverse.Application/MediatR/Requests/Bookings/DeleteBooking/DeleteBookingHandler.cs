using Cineverse.Mongo.Repositories.Booking;
using MediatR;

namespace Cineverse.Application.MediatR.Requests.Bookings.DeleteBooking;

public class DeleteBookingHandler(
    IBookingRepository bookingRepository
) : IRequestHandler<DeleteBookingRequest, bool>
{
    public async Task<bool> Handle(DeleteBookingRequest request, CancellationToken cancellationToken)
    {
        await bookingRepository.DeleteByIdAsync(request.Id, cancellationToken);
        return true;
    }
}