using Infrastructure.Mongo.Attributes;
using Infrastructure.Mongo.Models.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Cineverse.Mongo.Schemas.Entities;

[MongoCollection("screenings")]
public record ScreeningEntity : TimestampedEntity
{
    [BsonRepresentation(BsonType.ObjectId)]
    public required string MovieId { get; init; }
    
    [BsonRepresentation(BsonType.ObjectId)]
    public required string HallId { get; init; }
    
    [BsonRepresentation(BsonType.String)]
    public DateOnly Date { get; init; }
    
    [BsonRepresentation(BsonType.String)]
    public TimeOnly StartTime { get; init; }
    
    [BsonRepresentation(BsonType.String)]
    public TimeOnly EndTime { get; init; }
    
    public int TicketPrice { get; init; }
}