using MediatR;

namespace Cineverse.Application.MediatR.Requests.Movies.CreateMovie;

public record CreateMovieRequest(
    string Title,
    string Genre,
    string Description,
    string PosterUrl,
    string TrailerUrl,
    DateOnly ReleaseDate,
    int Duration
) : IRequest<string>;