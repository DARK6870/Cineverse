using IdentityService.Mongo.Schemas.Enums;
using Infrastructure.Mongo.Attributes;
using Infrastructure.Mongo.Models.Entities;

namespace IdentityService.Mongo.Schemas.Entities;

[MongoCollection("users")]
public class UserEntity : BaseEntity
{
    public required string Email { get; set; }
    
    public required string FirstName { get; set; }
    
    public required string LastName { get; set; }

    public Role Role { get; set; } = Role.User;

    public UserStatus Status { get; set; } = UserStatus.PendingEmailConfirmation;
    
    public string? PasswordHash { get; set; }
    
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    
    public DateTime DateUpdated { get; set; } = DateTime.UtcNow;

    public string GetFullName()
    {
        return $"{FirstName} {LastName}";
    }
}