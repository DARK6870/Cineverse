using Cineverse.Mongo.Common.Attributes;
using Cineverse.Mongo.Schemas.Base;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Cineverse.Mongo.Schemas.Entities;

[MongoCollection("Bookings")]
public class BookingEntity : BaseEntity
{
    [BsonRepresentation(BsonType.ObjectId)]
    public required string UserId { get; set; }

    [BsonRepresentation(BsonType.ObjectId)]
    public required string ScreeningId { get; set; }

    public required string[] SeatIds { get; set; }
    
    public required int TotalPrice { get; set; }

    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
}