using Cineverse.Mongo.Repositories.Interfaces;
using Cineverse.Mongo.Schemas.Entities;
using MediatR;

namespace Cineverse.Application.MediatR.Movies.Commands;

public record CreateMovieRequest(
    string Title,
    string Description,
    string[] Images,
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
            Description = request.Description,
            Duration = request.Duration,
            ReleaseDate = request.ReleaseDate,
            Images = request.Images
        };

        await movieRepository.InsertOneAsync(movie, cancellationToken);
        return true;
    }
}