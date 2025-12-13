using Infrastructure.Mongo.Attributes;
using Infrastructure.Mongo.Models.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Cineverse.Mongo.Schemas.Entities;

[MongoCollection("Screenings")]
public class ScreeningEntity : BaseEntity
{
    [BsonRepresentation(BsonType.ObjectId)]
    public required string MovieId { get; set; }
    
    [BsonRepresentation(BsonType.ObjectId)]
    public required string HallId { get; set; }
    
    [BsonRepresentation(BsonType.String)]
    public DateOnly Date { get; set; }
    
    [BsonRepresentation(BsonType.String)]
    public TimeOnly StartTime { get; set; }
    
    [BsonRepresentation(BsonType.String)]
    public TimeOnly EndTime { get; set; }
    
    public int TicketPrice { get; set; }
    
    public DateTime DateCreated  { get; set; } = DateTime.UtcNow;
}