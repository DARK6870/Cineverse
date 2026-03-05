using Infrastructure.Mongo.Attributes;
using Infrastructure.Mongo.Models.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace IdentityService.Mongo.Schemas.Entities;

[MongoCollection("refreshTokens")]
public record RefreshTokenEntity : BaseEntity
{
    [BsonRepresentation(BsonType.ObjectId)]
    public required string UserId { get; init; }
    
    public required string IpAddress { get; init; }
    
    public required string TokenHash { get; init; }
    
    public DateTime DateCreated { get; init; } = DateTime.UtcNow;
    
    public DateTime DateExpired { get; init; } = DateTime.UtcNow.AddDays(14);
}