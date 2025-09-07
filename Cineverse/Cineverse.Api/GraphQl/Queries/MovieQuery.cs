using Cineverse.Api.GraphQl.Base;
using Cineverse.Application.MediatR.Requests.Movies.GetMovies;
using Cineverse.Infrastructure.Common.Constants;
using Cineverse.Mongo.Schemas.Entities;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using MediatR;

namespace Cineverse.Api.GraphQl.Queries;

[ExtendObjectType(nameof(BaseGraphQlQuery))]
public class MovieQuery
{
    [UseOffsetPaging(ProviderName = GraphQlConstants.QueryablePaginationProvider)]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public Task<IQueryable<MovieEntity>> GetMovies(
        [Service] IMediator mediator,
        CancellationToken cancellationToken
    )
    {
        return mediator.Send(new GetMoviesRequest(), cancellationToken);
    }
}