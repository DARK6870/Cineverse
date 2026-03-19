namespace AutomationTests.Models.Cineverse.Entities;

public record ScreeningEntity
{
    public required string Id { get; init; }
    
    public required string MovieId { get; init; }
    
    public required string HallId { get; init; }
    
    public DateOnly Date { get; init; }
    
    public TimeOnly StartTime { get; init; }
    
    public TimeOnly EndTime { get; init; }
    
    public int TicketPrice { get; init; }
    
    public DateTime DateCreated  { get; init; } = DateTime.UtcNow;
}