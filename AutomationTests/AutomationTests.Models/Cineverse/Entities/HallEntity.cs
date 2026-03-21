using AutomationTests.Models.Cineverse.Models;
using Infrastructure.Mongo.Models.Entities;

namespace AutomationTests.Models.Cineverse.Entities;

public record HallEntity : TimestampedEntity
{
    public required string Name { get; init; }
    
    public required Seat[] Seats { get; init; }
}