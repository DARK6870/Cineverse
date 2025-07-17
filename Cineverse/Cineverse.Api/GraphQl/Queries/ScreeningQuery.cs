using Cineverse.API.GraphQl.Base;
using Cineverse.Application.MediatR.Screenings.Queries;
using Cineverse.Infrastructure.Common.Constants;
using Cineverse.Mongo.Schemas.Entities;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using MediatR;

namespace Cineverse.API.GraphQl.Queries;

[ExtendObjectType(nameof(BaseGraphQlQuery))]
public class ScreeningQuery
{
    [UseOffsetPaging(ProviderName = GraphQlConstants.QueryablePaginationProvider)]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public Task<IQueryable<ScreeningEntity>> GetScreenings(
        [Service] IMediator mediator
    )
    {
        return mediator.Send(new GetScreeningsRequest());
    }

    public async Task<ScreeningEntity?> GetScreeningById(
        [Service] IMediator mediator,
        string id
    )
    {
        return await mediator.Send(new GetScreeningByIdRequest(id));
    }
}