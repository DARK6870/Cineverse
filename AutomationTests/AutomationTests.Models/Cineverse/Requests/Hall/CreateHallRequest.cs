namespace AutomationTests.Models.Cineverse.Requests.Hall;

public record CreateHallRequest(
    string Name,
    List<SeatRequest> Seats
);