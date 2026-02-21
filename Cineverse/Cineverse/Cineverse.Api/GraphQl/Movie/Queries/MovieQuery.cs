using Cineverse.Application.MediatR.Requests.Movies.GetGenreDistinctFilterValues;
using Cineverse.Application.MediatR.Requests.Movies.GetMovieById;
using Cineverse.Application.MediatR.Requests.Movies.GetMovies;
using Cineverse.Mongo.Schemas.Entities;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using Infrastructure.WebApi.GraphQl.Base;
using Infrastructure.WebApi.GraphQl.Constants;
using MediatR;

namespace Cineverse.Api.GraphQl.Movie.Queries;

[ExtendObjectType(nameof(BaseGraphQlQuery))]
public class MovieQuery
{
    [UseOffsetPaging(ProviderName = GraphQlConstants.QueryablePaginationProvider)]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public Task<IQueryable<MovieEntity>> GetMovies(
        string? searchTerm,
        [Service] IMediator mediator,
        CancellationToken cancellationToken
    )
    {
        return mediator.Send(new GetMoviesRequest(searchTerm), cancellationToken);
    }

    public async Task<MovieEntity?> GetMovieById(
        [Service] IMediator mediator,
        string id,
        CancellationToken cancellationToken
    )
    {
        return await mediator.Send(new GetMovieByIdRequest(id), cancellationToken);
    }

    public async Task<IEnumerable<string>> GetGenreDistinctFilterValues(
        [Service] IMediator mediator,
        CancellationToken cancellationToken
    )
    {
        return await mediator.Send(new GetGenreDistinctFilterValuesRequest(), cancellationToken);
    }
}