using Cineverse.Mongo.Schemas.Enums;

namespace Cineverse.Infrastructure.Services.Interfaces;

public interface IUserContext
{
    string UserId { get; }
    
    UserStatus UserStatus { get; }
    
    string UserName { get; }
    
    string Email { get; }
    
    Role Role { get; }
    
    string IpAddress { get; }
}