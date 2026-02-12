using Auth.Models.Enums;

namespace Infrastructure.Context.UserContext;

public interface IUserContext
{
    string UserId { get; }
    
    UserStatus UserStatus { get; }
    
    string UserName { get; }
    
    string Email { get; }
    
    Role Role { get; }
    
    string IpAddress { get; }
}