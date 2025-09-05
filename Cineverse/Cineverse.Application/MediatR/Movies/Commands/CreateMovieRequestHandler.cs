using Cineverse.Mongo.Repositories.Movie;
using Cineverse.Mongo.Schemas.Entities;
using MediatR;

namespace Cineverse.Application.MediatR.Movies.Commands;

public record CreateMovieRequest(
    string Title,
    string Genre,
    string Description,
    string PosterUrl,
    string TrailerUrl,
    DateOnly ReleaseDate,
    int Duration
) : IRequest<bool>;

public class CreateMovieRequestHandler(
    IMovieRepository movieRepository
) : IRequestHandler<CreateMovieRequest, bool>
{
    public async Task<bool> Handle(CreateMovieRequest request, CancellationToken cancellationToken)
    {
        var movie = new MovieEntity
        {
            Title = request.Title,
            Genre = request.Genre,
            Description = request.Description,
            Duration = request.Duration,
            ReleaseDate = request.ReleaseDate,
            PosterUrl = request.PosterUrl,
            TrailerUrl = request.TrailerUrl
        };

        await movieRepository.InsertOneAsync(movie, cancellationToken);
        return true;
    }
}