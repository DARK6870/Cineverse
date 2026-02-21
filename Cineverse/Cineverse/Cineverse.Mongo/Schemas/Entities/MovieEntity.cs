using Infrastructure.Mongo.Attributes;
using Infrastructure.Mongo.Models.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Cineverse.Mongo.Schemas.Entities;

[MongoCollection("movies")]
public record MovieEntity : TimestampedEntity
{
    public required string Title { get; init; }
    
    public required string Genre { get; init; }
    
    public required string Description { get; init; }
    
    public required string PosterUrl { get; init; }
    
    public required string TrailerUrl { get; init; }
    
    [BsonRepresentation(BsonType.String)]
    public required DateOnly ReleaseDate { get; init; }
    
    public required int Duration { get; init; }

    public bool IsAvailable { get; init; } = true;
}