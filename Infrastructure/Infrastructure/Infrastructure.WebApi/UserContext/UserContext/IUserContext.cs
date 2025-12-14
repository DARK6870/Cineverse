namespace Infrastructure.WebApi.UserContext.UserContext;

public interface IUserContext
{
    string UserId { get; }
    
    string UserStatus { get; }
    
    string UserName { get; }
    
    string Email { get; }
    
    string Role { get; }
    
    string IpAddress { get; }
}