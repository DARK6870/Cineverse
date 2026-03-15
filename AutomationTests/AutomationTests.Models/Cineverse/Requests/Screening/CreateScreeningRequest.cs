namespace AutomationTests.Models.Cineverse.Requests.Screening;

public record CreateScreeningRequest(
    string MovieId,
    string HallId,
    DateOnly Date,
    TimeOnly StartTime,
    TimeOnly EndTime,
    int TicketPrice
);