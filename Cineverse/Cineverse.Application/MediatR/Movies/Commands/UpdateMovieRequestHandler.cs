using System.Net;
using Cineverse.Infrastructure.Common.Exceptions;
using Cineverse.Mongo.Repositories.Interfaces;
using Cineverse.Mongo.Schemas.Entities;
using MediatR;

namespace Cineverse.Application.MediatR.Movies.Commands;

public record UpdateMovieRequest(
    string Id,
    string Title,
    string Description,
    string[] Images,
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
        var movie = await movieRepository.FindByIdAsync(request.Id, cancellationToken)
                    ?? throw new ApiRequestException("Movie not found", HttpStatusCode.NotFound);

        var updatedMovie = new MovieEntity
        {
            Id = request.Id,
            Title = request.Title,
            Description = request.Description,
            Images = request.Images,
            ReleaseDate = request.ReleaseDate,
            Duration = request.Duration,
            IsAvailable = request.IsAvailable,
            DateCreated = movie.DateCreated
        };
        await movieRepository.ReplaceOneAsync(updatedMovie, cancellationToken);
        
        return true;
    }
}