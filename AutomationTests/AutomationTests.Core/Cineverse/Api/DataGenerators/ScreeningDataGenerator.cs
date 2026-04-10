using AutomationTests.Models.Cineverse.Requests.Screening;

namespace AutomationTests.Core.Cineverse.Api.DataGenerators;

public static class ScreeningDataGenerator
{
    public static CreateScreeningRequest ValidCreateScreeningRequest(string movieId, string hallId)
    {
        return new CreateScreeningRequest(
            movieId,
            hallId,
            DateOnly.FromDateTime(DateTime.Now.AddDays(7)),
            TimeOnly.FromDateTime(DateTime.Now),
            TimeOnly.FromDateTime(DateTime.Now.AddHours(2)),
            10
        );
    }
    
    public static CreateScreeningRequest InvalidCreateScreeningRequest()
    {
        return new CreateScreeningRequest(
            "",
            "",
            DateOnly.FromDateTime(DateTime.Now),
            TimeOnly.FromDateTime(DateTime.Now.AddHours(2)),
            TimeOnly.FromDateTime(DateTime.Now),
            0
        );
    }
}