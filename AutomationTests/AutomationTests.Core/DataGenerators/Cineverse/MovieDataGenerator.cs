using AutomationTests.Models.Cineverse.Requests.Movie;
using MongoDB.Bson;

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

    public static CreateMovieRequest InvalidCreateMovieRequest()
    {
        return new CreateMovieRequest(
            "",
            "",
            "",
            "",
            "",
            DateOnly.FromDateTime(DateTime.Now),
            -1
        );
    }

    public static UpdateMovieRequest ValidUpdateMovieRequest(string? movieId = null)
    {
        return new UpdateMovieRequest(
            movieId ?? ObjectId.GenerateNewId().ToString(),
            "Updated title",
            "Updated genre",
            "Updated description",
            "https://test.com/updated.png",
            "https://test.com/updated.mp4",
            DateOnly.FromDateTime(DateTime.UtcNow),
            90,
            true
        );
    }

    public static UpdateMovieRequest InvalidUpdateMovieRequest()
    {
        return new UpdateMovieRequest(
            "",
            "",
            "",
            "",
            "",
            "",
            DateOnly.FromDateTime(DateTime.UtcNow),
            -1,
            true
        );
    }
}