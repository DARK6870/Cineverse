using Cineverse.Mongo.Repositories.Interfaces;
using Cineverse.Mongo.Schemas.Entities;
using MediatR;

namespace Cineverse.Application.MediatR.Bookings.Queries;

public record GetBookingsRequest : IRequest<IQueryable<BookingEntity>>;

public class GetBookingsRequestHandler(
    IBookingRepository bookingRepository
) : IRequestHandler<GetBookingsRequest, IQueryable<BookingEntity>>
{
    public Task<IQueryable<BookingEntity>> Handle(GetBookingsRequest request, CancellationToken cancellationToken)
    {
        return Task.FromResult(bookingRepository.AsQueryable());
    }
}