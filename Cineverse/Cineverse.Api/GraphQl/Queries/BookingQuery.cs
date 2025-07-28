using Cineverse.API.GraphQl.Base;
using Cineverse.Application.MediatR.Bookings.Queries;
using Cineverse.Infrastructure.Common.Constants;
using Cineverse.Mongo.Schemas.Entities;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using MediatR;

namespace Cineverse.API.GraphQl.Queries;

[ExtendObjectType(nameof(BaseGraphQlQuery))]
public class BookingQuery
{
    [UseOffsetPaging(ProviderName = GraphQlConstants.QueryablePaginationProvider)]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public Task<IQueryable<BookingEntity>> GetBookings(
        [Service] IMediator mediator,
        CancellationToken cancellationToken
    )
    {
        return mediator.Send(new GetBookingsRequest(), cancellationToken);
    }

    public async Task<BookingEntity?> GetBookingById(
        [Service] IMediator mediator,
        string id,
        CancellationToken cancellationToken
    )
    {
        return await mediator.Send(new GetBookingByIdRequest(id), cancellationToken);
    }
}