using AutomationTests.Models.Cineverse.Requests.Screening;

namespace AutomationTests.Core.DataGenerators.Cineverse;

public static class ScreeningDataGenerator
{
    public static CreateScreeningRequest ValidCreateScreeningRequest(string? movieId, string? hallId = null)
    {
        return new CreateScreeningRequest(
            "",
            hallId ?? "test",// TODO: use default hall id
            DateOnly.FromDateTime(DateTime.Now),
            TimeOnly.FromDateTime(DateTime.Now),
            TimeOnly.FromDateTime(DateTime.Now),
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