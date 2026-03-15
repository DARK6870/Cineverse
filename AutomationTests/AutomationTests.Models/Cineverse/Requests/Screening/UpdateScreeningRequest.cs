namespace AutomationTests.Models.Cineverse.Requests.Screening;

public record UpdateScreeningRequest(
    string Id,
    string MovieId,
    string HallId,
    DateOnly Date,
    TimeOnly StartTime,
    TimeOnly EndTime,
    int TicketPrice
);