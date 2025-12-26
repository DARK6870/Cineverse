using Auth.Models.Enums;

namespace Infrastructure.Context.UserContext;

public class UserContext : IUserContext
{
    public string UserId { get; set; } = string.Empty;
    
    public string UserName { get; set; } = string.Empty;
    
    public string Email { get; set; } = string.Empty;
    
    public UserStatus UserStatus { get; set; } = UserStatus.Normal;
    
    public Role Role { get; set; } = Role.User;
    
    public string IpAddress { get; set; } = string.Empty;
}