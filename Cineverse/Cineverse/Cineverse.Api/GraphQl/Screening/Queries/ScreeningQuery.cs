using Cineverse.Application.MediatR.Requests.Screenings.GetScreeningById;
using Cineverse.Application.MediatR.Requests.Screenings.GetScreenings;
using Cineverse.Mongo.Schemas.Entities;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using Infrastructure.WebApi.GraphQl.Base;
using Infrastructure.WebApi.GraphQl.Constants;
using MediatR;

namespace Cineverse.Api.GraphQl.Screening.Queries;

[ExtendObjectType(nameof(BaseGraphQlQuery))]
public class ScreeningQuery
{
    [UseOffsetPaging(ProviderName = GraphQlConstants.QueryablePaginationProvider)]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public Task<IQueryable<ScreeningEntity>> GetScreenings(
        [Service] IMediator mediator,
        CancellationToken cancellationToken
    )
    {
        return mediator.Send(new GetScreeningsRequest(), cancellationToken);
    }

    public async Task<ScreeningEntity?> GetScreeningById(
        [Service] IMediator mediator,
        string id,
        CancellationToken cancellationToken
    )
    {
        return await mediator.Send(new GetScreeningByIdRequest(id), cancellationToken);
    }
}