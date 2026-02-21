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
        if (string.IsNullOrEmpty(request.SearchTerm))
            return Task.FromResult(movieRepository.AsQueryable());

        return Task.FromResult(movieRepository.SearchByText(
                request.SearchTerm,
                x => x.Description,
                x => x.TrailerUrl,
                x => x.PosterUrl
            )
        );
    }
}