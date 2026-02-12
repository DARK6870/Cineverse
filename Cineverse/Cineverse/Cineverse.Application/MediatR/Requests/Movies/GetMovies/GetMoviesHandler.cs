using Cineverse.Mongo.Repositories.Movie;
using Cineverse.Mongo.Schemas.Entities;
using MediatR;

namespace Cineverse.Application.MediatR.Requests.Movies.GetMovies;

public class GetMoviesHandler(
    IMovieRepository movieRepository
) : IRequestHandler<GetMoviesRequest, IQueryable<MovieEntity>>
{
    public Task<IQueryable<MovieEntity>> Handle(GetMoviesRequest request, CancellationToken cancellationToken)
    {
        return Task.FromResult(movieRepository.AsQueryable());
    }
}