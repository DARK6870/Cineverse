using Auth.Models.Enums;
using Infrastructure.Context.Constants;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Context.UserContext;

public class UserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
{
    private HttpContext Http => httpContextAccessor.HttpContext!;

    public string UserId => Http.Items[UserContextKeys.UserId] as string ?? string.Empty;
    public string UserName => Http.Items[UserContextKeys.UserName] as string ?? string.Empty;
    public string Email => Http.Items[UserContextKeys.Email] as string ?? string.Empty;
    public Role Role => Http.Items[UserContextKeys.Role] is Role r ? r : Role.User;
    public UserStatus UserStatus => Http.Items[UserContextKeys.UserStatus] is UserStatus s ? s : UserStatus.Normal;
    public string IpAddress => Http.Items[UserContextKeys.IpAddress] as string ?? string.Empty;
}