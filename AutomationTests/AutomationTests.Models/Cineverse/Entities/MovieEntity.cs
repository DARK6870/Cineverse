using Infrastructure.Mongo.Models.Entities;

namespace AutomationTests.Models.Cineverse.Entities;

public record MovieEntity : TimestampedEntity
{
    public required string Title { get; init; }
    
    public required string Genre { get; init; }
    
    public required string Description { get; init; }
    
    public required string PosterUrl { get; init; }
    
    public required string TrailerUrl { get; init; }
    
    public required DateOnly ReleaseDate { get; init; }
    
    public required int Duration { get; init; }

    public bool IsAvailable { get; init; }
}