using Cineverse.Mongo.Common.Attributes;
using Cineverse.Mongo.Schemas.Base;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Cineverse.Mongo.Schemas.Entities;

[MongoCollection("Movies")]
public class MovieEntity : BaseEntity
{
    public required string Title { get; set; }
    
    public required string Genre { get; set; }
    
    public required string Description { get; set; }
    
    public required string PosterUrl { get; set; }
    
    public required string TrailerUrl { get; set; }
    
    [BsonRepresentation(BsonType.String)]
    public required DateOnly ReleaseDate { get; set; }
    
    public required int Duration { get; set; }

    public bool IsAvailable { get; set; } = true;

    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
}