using Cineverse.Application.MediatR.Requests.Hall.GetHallById;
using Cineverse.Application.MediatR.Requests.Hall.GetHalls;
using Cineverse.Mongo.Schemas.Entities;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using Infrastructure.WebApi.GraphQl.Base;
using Infrastructure.WebApi.GraphQl.Constants;
using MediatR;

namespace Cineverse.Api.GraphQl.Queries;

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