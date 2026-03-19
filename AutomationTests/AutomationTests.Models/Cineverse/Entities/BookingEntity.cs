namespace AutomationTests.Models.Cineverse.Entities;

public record BookingEntity
{
    public required string Id { get; init; }
    
    public required string UserId { get; init; }

    public required string ScreeningId { get; init; }

    public required string[] SeatIds { get; init; }
    
    public required int TotalPrice { get; init; }
}