using Cineverse.Mongo.Common.Attribues;
using Cineverse.Mongo.Schemas.Base;

namespace Cineverse.Mongo.Schemas.Entities;

[MongoCollection("RefreshTokens")]
public class RefreshTokenEntity : BaseEntity
{
    public required string UserId { get; set; }
    
    public required string Token { get; set; }
    
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    
    public DateTime DateExpired { get; set; } = DateTime.UtcNow.AddDays(14);
}