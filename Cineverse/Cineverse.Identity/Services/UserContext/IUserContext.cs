using Cineverse.Mongo.Schemas.Enums;

namespace Cineverse.Identity.Services.UserContext;

public interface IUserContext
{
    string UserId { get; }
    
    UserStatus UserStatus { get; }
    
    string UserName { get; }
    
    string Email { get; }
    
    Role Role { get; }
    
    string IpAddress { get; }
}