using Infrastructure.Mongo.Attributes;
using Infrastructure.Mongo.Models.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Cineverse.Mongo.Schemas.Entities;

[MongoCollection("bookings")]
public record BookingEntity : BaseEntity
{
    [BsonRepresentation(BsonType.ObjectId)]
    public required string UserId { get; init; }

    [BsonRepresentation(BsonType.ObjectId)]
    public required string ScreeningId { get; init; }

    public required string[] SeatIds { get; init; }
    
    public required int TotalPrice { get; init; }

    public DateTime DateCreated { get; init; } = DateTime.UtcNow;
}