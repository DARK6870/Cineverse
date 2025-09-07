using MediatR;

namespace Cineverse.Application.MediatR.Requests.Movies.UpdateMovie;

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