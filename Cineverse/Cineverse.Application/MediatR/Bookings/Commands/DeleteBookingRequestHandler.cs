using Cineverse.Mongo.Repositories.Interfaces;
using MediatR;

namespace Cineverse.Application.MediatR.Bookings.Commands;

public record DeleteBookingRequest(string Id) : IRequest<bool>;

public class DeleteBookingRequestHandler(
    IBookingRepository bookingRepository
) : IRequestHandler<DeleteBookingRequest, bool>
{
    public async Task<bool> Handle(DeleteBookingRequest request, CancellationToken cancellationToken)
    {
        await bookingRepository.DeleteByIdAsync(request.Id, cancellationToken);
        return true;
    }
}