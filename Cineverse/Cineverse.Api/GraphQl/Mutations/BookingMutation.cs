using Cineverse.Api.GraphQl.Base;
using Cineverse.Application.MediatR.Bookings.Commands;
using HotChocolate;
using HotChocolate.Types;
using MediatR;

namespace Cineverse.Api.GraphQl.Mutations;

[ExtendObjectType(nameof(BaseGraphQlMutation))]
public class BookingMutation
{
    public async Task<bool> CreateBooking(
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