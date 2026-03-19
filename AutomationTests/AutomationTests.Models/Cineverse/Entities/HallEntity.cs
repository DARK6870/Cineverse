using AutomationTests.Models.Cineverse.Models;

namespace AutomationTests.Models.Cineverse.Entities;

public record HallEntity
{
    public required string Id { get; init; }
    
    public required string Name { get; init; }
    
    public required Seat[] Seats { get; init; }
}