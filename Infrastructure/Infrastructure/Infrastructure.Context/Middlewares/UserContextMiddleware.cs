using System.Security.Claims;
using Auth.Constants;
using Infrastructure.Context.UserContext;
using Microsoft.AspNetCore.Http;
using Auth.Models.Enums;

namespace Infrastructure.Context.Middlewares;

public class UserContextMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, IUserContext userContext)
    {
        if (context.User.Identity?.IsAuthenticated is true && userContext is UserContext.UserContext ctx)
        {
            ctx.UserId = context.User.FindFirst(JwtClaimTypes.UserIdClaimType)?.Value ?? string.Empty;
            
            ctx.UserName = context.User.Identity?.Name ?? string.Empty;
            
            ctx.Email = context.User.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty;
            
            ctx.Role = ParseEnumClaim(context.User, ClaimTypes.Role, Role.User);
            
            ctx.UserStatus = ParseEnumClaim(context.User, JwtClaimTypes.UserStatusClaimType, UserStatus.Normal);
            
            ctx.IpAddress = context.Connection.RemoteIpAddress?.ToString()
                            ?? context.Request.Headers["X-Forwarded-For"].FirstOrDefault()
                            ?? string.Empty;
        }

        await next(context);
    }

    private static T ParseEnumClaim<T>(ClaimsPrincipal user, string claimType, T defaultValue) where T : struct
    {
        var claimValue = user.FindFirst(claimType)?.Value;
        return !string.IsNullOrEmpty(claimValue) && Enum.TryParse<T>(claimValue, true, out var result)
            ? result
            : defaultValue;
    }
}
