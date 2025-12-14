using Cineverse.Application.MediatR.Requests.Movies.GetMovieById;
using Cineverse.Application.MediatR.Requests.Movies.GetMovies;
using Infrastructure.WebApi.GraphQl.Constants;
using Cineverse.Mongo.Schemas.Entities;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using Infrastructure.WebApi.GraphQl.Base;
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

    public async Task<MovieEntity?> GetMovieById(
        [Service] IMediator mediator,
        string id,
        CancellationToken cancellationToken
    )
    {
        return await mediator.Send(new GetMovieByIdRequest(id), cancellationToken);
    }
}