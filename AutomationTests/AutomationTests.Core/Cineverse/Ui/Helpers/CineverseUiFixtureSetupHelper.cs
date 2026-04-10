using AutomationTests.Models.Cineverse.Entities;
using AutomationTests.Models.Cineverse.Requests.Movie;
using AutomationTests.Models.Cineverse.Requests.Screening;
using CineverseClient = AutomationTests.Core.Cineverse.Api.Client.CineverseClient;

namespace AutomationTests.Core.Cineverse.Ui.Helpers;

public static class CineverseUiFixtureSetupHelper
{
    public static async Task<IReadOnlyList<SeededMovie>> SeedMoviesWithScreeningsAsync(
        CineverseClient adminClient,
        HallEntity defaultHall)
    {
        CreateMovieRequest[] requests =
        [
            new("UI Test Movie Alpha", "Action", "Seeded for UI tests", "https://test.com/poster.png", "https://test.com/trailer.mp4", DateOnly.FromDateTime(DateTime.UtcNow), 120),
            new("UI Test Movie Beta",  "Drama",  "Seeded for UI tests", "https://test.com/poster.png", "https://test.com/trailer.mp4", DateOnly.FromDateTime(DateTime.UtcNow), 95),
            new("UI Test Movie Gamma", "Sci-Fi", "Seeded for UI tests", "https://test.com/poster.png", "https://test.com/trailer.mp4", DateOnly.FromDateTime(DateTime.UtcNow), 150),
        ];

        var seeded = new List<SeededMovie>(requests.Length);
        foreach (var req in requests)
        {
            var movieResponse = await adminClient.CreateMovie(req);
            if (!movieResponse.IsSuccess || movieResponse.Data is null) continue;

            var movieId = movieResponse.Data;

            await adminClient.CreateScreening(new CreateScreeningRequest(
                movieId,
                defaultHall.Id.ToString(),
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(3)),
                TimeOnly.FromDateTime(DateTime.UtcNow),
                TimeOnly.FromDateTime(DateTime.UtcNow.AddHours(2)),
                10
            ));

            seeded.Add(new SeededMovie(movieId, req.Title));
        }

        return seeded.AsReadOnly();
    }
}

public sealed record SeededMovie(string Id, string Title);
