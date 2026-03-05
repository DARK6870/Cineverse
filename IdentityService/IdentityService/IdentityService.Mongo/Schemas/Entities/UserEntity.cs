using Auth.Models.Enums;
using IdentityService.Mongo.Schemas.Enums;
using Infrastructure.Mongo.Attributes;
using Infrastructure.Mongo.Models.Entities;

namespace IdentityService.Mongo.Schemas.Entities;

[MongoCollection("users")]
public record UserEntity : TimestampedEntity
{
    public required string Email { get; init; }
    
    public required string FirstName { get; init; }
    
    public required string LastName { get; init; }

    public Role Role { get; init; } = Role.User;

    public UserStatus Status { get; init; } = UserStatus.PendingEmailConfirmation;
    
    public string? PasswordHash { get; init; }
    
    public AuthenticationProvider Provider { get; init; } = AuthenticationProvider.Identity;

    public string GetFullName()
    {
        return $"{FirstName} {LastName}";
    }
}