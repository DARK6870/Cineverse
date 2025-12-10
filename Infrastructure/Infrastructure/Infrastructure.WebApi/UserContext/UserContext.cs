namespace Infrastructure.WebApi.UserContext;

public class UserContext : IUserContext
{
    public string UserId { get; set; } = string.Empty;
    
    public string UserName { get; set; } = string.Empty;
    
    public string Email { get; set; } = string.Empty;
    
    public string UserStatus { get; set; } = string.Empty;
    
    public string Role { get; set; } = string.Empty;
    
    public string IpAddress { get; set; } = string.Empty;
}