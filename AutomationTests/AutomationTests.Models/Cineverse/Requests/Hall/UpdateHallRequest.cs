namespace AutomationTests.Models.Cineverse.Requests.Hall;

public record UpdateHallRequest(
    string Id,
    string Name,
    SeatRequest[] Seats
);