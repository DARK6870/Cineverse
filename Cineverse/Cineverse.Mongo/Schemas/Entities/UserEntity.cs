using Cineverse.Mongo.Common.Attributes;
using Cineverse.Mongo.Schemas.Base;
using Cineverse.Mongo.Schemas.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Cineverse.Mongo.Schemas.Entities;

[MongoCollection("Users")]
public class UserEntity : BaseEntity
{
    public required string Email { get; set; }
    
    public required string FirstName { get; set; }
    
    public required string LastName { get; set; }

    [BsonRepresentation(BsonType.String)]
    public Role Role { get; set; } = Role.User;

    [BsonRepresentation(BsonType.String)]
    public UserStatus Status { get; set; } = UserStatus.PendingEmailConfirmation;
    
    public string? PasswordHash { get; set; }
    
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    
    public DateTime DateUpdated { get; set; } = DateTime.UtcNow;
}