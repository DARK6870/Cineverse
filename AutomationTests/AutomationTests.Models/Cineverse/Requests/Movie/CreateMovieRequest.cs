using System.Text.Json.Serialization;

namespace AutomationTests.Models.Cineverse.Requests.Movie;

public record CreateMovieRequest(
    string Title,
    string Genre,
    string Description,
    string PosterUrl,
    string TrailerUrl,
    DateOnly ReleaseDate,
    int Duration
);