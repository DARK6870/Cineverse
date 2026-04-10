using AutomationTests.Models.Cineverse.Requests.Hall;

namespace AutomationTests.Core.Cineverse.Api.DataGenerators;

public static class HallDataGenerator
{
    public static readonly CreateHallRequest DefaultHall = new CreateHallRequest(
        "Cineverse Hall 1",
        [
            new SeatRequest(1, 1),
            new SeatRequest(1, 2),
            new SeatRequest(1, 3),
            new SeatRequest(1, 4),
            new SeatRequest(1, 5),
            new SeatRequest(1, 6),
            new SeatRequest(2, 1),
            new SeatRequest(2, 2),
            new SeatRequest(2, 3),
            new SeatRequest(2, 4),
            new SeatRequest(2, 5),
            new SeatRequest(2, 6),
            new SeatRequest(3, 1),
            new SeatRequest(3, 2),
            new SeatRequest(3, 3),
            new SeatRequest(3, 4),
            new SeatRequest(3, 5),
            new SeatRequest(3, 6),
            new SeatRequest(4, 1),
            new SeatRequest(4, 2),
            new SeatRequest(4, 3),
            new SeatRequest(4, 4),
            new SeatRequest(4, 5),
            new SeatRequest(4, 6),
        ]
    );
}