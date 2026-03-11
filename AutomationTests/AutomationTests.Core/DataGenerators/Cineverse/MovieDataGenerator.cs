using AutomationTests.Models.Cineverse.Requests.Movie;

namespace AutomationTests.Core.DataGenerators.Cineverse;

public static class MovieDataGenerator
{
    public static CreateMovieRequest ValidCreateMovieRequest()
    {
        return new CreateMovieRequest(
            Guid.NewGuid().ToString(),
            "Horror",
            "Test movie description",
            "https://test.com/poster.png",
            "https://test.com/trailer.mp4",
            DateOnly.FromDateTime(DateTime.Now),
            120
        );
    }
}