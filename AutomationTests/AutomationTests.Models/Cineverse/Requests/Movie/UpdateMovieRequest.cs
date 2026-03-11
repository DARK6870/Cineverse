namespace AutomationTests.Models.Cineverse.Requests.Movie;

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
);