using Cineverse.Mongo.Repositories.Movie;
using Cineverse.Mongo.Schemas.Entities;
using MediatR;

namespace Cineverse.Application.MediatR.Requests.Movies.UpdateMovie;

public class UpdateMovieHandler(
    IMovieRepository movieRepository
) : IRequestHandler<UpdateMovieRequest, bool>
{
    public async Task<bool> Handle(UpdateMovieRequest request, CancellationToken cancellationToken)
    {
        var movie = await movieRepository.FindByIdOrThrowAsync(request.Id, cancellationToken);
        
        var updatedMovie = new MovieEntity
        {
            Id = request.Id,
            Title = request.Title,
            Genre = request.Genre,
            Description = request.Description,
            PosterUrl = request.PosterUrl,
            TrailerUrl = request.TrailerUrl,
            ReleaseDate = request.ReleaseDate,
            Duration = request.Duration,
            IsAvailable = request.IsAvailable,
            DateCreated = movie.DateCreated
        };
        await movieRepository.ReplaceOneAsync(updatedMovie, cancellationToken);
        
        return true;
    }
}