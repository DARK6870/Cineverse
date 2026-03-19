namespace AutomationTests.Models.Cineverse.Models;

public record Seat
{
    public required string SeatId { get; init; }
    
    public int Row { get; init; }
    
    public int Number { get; init; }
}