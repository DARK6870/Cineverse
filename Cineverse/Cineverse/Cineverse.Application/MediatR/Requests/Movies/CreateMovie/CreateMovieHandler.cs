using Cineverse.Mongo.Repositories.Movie;
using Cineverse.Mongo.Schemas.Entities;
using MediatR;

namespace Cineverse.Application.MediatR.Requests.Movies.CreateMovie;

public class CreateMovieHandler(
    IMovieRepository movieRepository
) : IRequestHandler<CreateMovieRequest, string>
{
    public async Task<string> Handle(CreateMovieRequest request, CancellationToken cancellationToken)
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
        return movie.Id;
    }
}