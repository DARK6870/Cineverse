using Cineverse.API.GraphQl.Base;
using Cineverse.Application.MediatR.Hall.Queries;
using Cineverse.Application.MediatR.Screenings.Queries;
using Cineverse.Infrastructure.Common.Constants;
using Cineverse.Mongo.Schemas.Entities;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using MediatR;

namespace Cineverse.API.GraphQl.Queries;

[ExtendObjectType(nameof(BaseGraphQlQuery))]
public class HallQuery
{
    [UseOffsetPaging(ProviderName = GraphQlConstants.QueryablePaginationProvider)]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public Task<IQueryable<HallEntity>> GetHalls(
        [Service] IMediator mediator,
        CancellationToken cancellationToken
    )
    {
        return mediator.Send(new GetHallsRequest(), cancellationToken);
    }

    public async Task<HallEntity?> GetHallById(
        [Service] IMediator mediator,
        string id,
        CancellationToken cancellationToken
    )
    {
        return await mediator.Send(new GetHallByIdRequest(id), cancellationToken);
    }
}