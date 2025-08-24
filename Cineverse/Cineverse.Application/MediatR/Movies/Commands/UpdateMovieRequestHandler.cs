using System.Net;
using Cineverse.Infrastructure.Common.Exceptions;
using Cineverse.Mongo.Repositories.Interfaces;
using Cineverse.Mongo.Schemas.Entities;
using MediatR;

namespace Cineverse.Application.MediatR.Movies.Commands;

public record UpdateMovieRequest(
    string Id,
    string Title,
    string Genre,
    string Description,
    string PosterUrl,
    string TrailerUrl,
    DateOnly ReleaseDate,
    int Duration,
    bool IsAvailable
) : IRequest<bool>;

public class UpdateMovieRequestHandler(
    IMovieRepository movieRepository
) : IRequestHandler<UpdateMovieRequest, bool>
{
    public async Task<bool> Handle(UpdateMovieRequest request, CancellationToken cancellationToken)
    {
        // TODO: refactor all same moments
        var movie = await movieRepository.FindByIdAsync(request.Id, cancellationToken)
                    ?? throw new ApiRequestException("Movie not found", HttpStatusCode.NotFound);

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