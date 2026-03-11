using Cineverse.Application.MediatR.Requests.Bookings.CreateBooking;
using Cineverse.Application.MediatR.Requests.Bookings.DeleteBooking;
using HotChocolate;
using HotChocolate.Types;
using Infrastructure.WebApi.GraphQl.Base;
using MediatR;

namespace Cineverse.Api.GraphQl.Booking.Mutations;

[ExtendObjectType(nameof(BaseGraphQlMutation))]
public class BookingMutation
{
    public async Task<string> CreateBooking(
        [Service] IMediator mediator,
        CreateBookingRequest request,
        CancellationToken cancellationToken
    )
    {
        return await mediator.Send(request, cancellationToken);
    }
    
    public async Task<bool> DeleteBooking(
        [Service] IMediator mediator,
        string id,
        CancellationToken cancellationToken
    )
    {
        return await mediator.Send(new DeleteBookingRequest(id), cancellationToken);
    }
}