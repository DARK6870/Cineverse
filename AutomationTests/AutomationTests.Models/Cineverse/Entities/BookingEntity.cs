using Infrastructure.Mongo.Models.Entities;

namespace AutomationTests.Models.Cineverse.Entities;

public record BookingEntity : BaseEntity
{
    public required string UserId { get; init; }

    public required string ScreeningId { get; init; }

    public required string[] SeatIds { get; init; }
    
    public required int TotalPrice { get; init; }
    
    public DateTime DateCreated { get; init; }
}