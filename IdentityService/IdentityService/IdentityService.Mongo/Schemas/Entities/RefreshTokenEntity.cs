using Infrastructure.Mongo.Attributes;
using Infrastructure.Mongo.Models.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace IdentityService.Mongo.Schemas.Entities;

[MongoCollection("refreshTokens")]
public class RefreshTokenEntity : BaseEntity
{
    [BsonRepresentation(BsonType.ObjectId)]
    public required string UserId { get; set; }
    
    public required string IpAddress { get; set; }
    
    public required string Token { get; set; }
    
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    
    public DateTime DateExpired { get; set; } = DateTime.UtcNow.AddDays(14);
}