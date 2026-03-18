using System.Security.Claims;
using Auth.Constants;
using Infrastructure.Context.UserContext;
using Microsoft.AspNetCore.Http;
using Auth.Models.Enums;
using Infrastructure.Context.Constants;
using Infrastructure.Context.Helpers;

namespace Infrastructure.Context.Middlewares;

public class UserContextMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, IUserContext userContext)
    {
        if (context.User.Identity?.IsAuthenticated is true)
        {
            context.Items[UserContextKeys.UserId] = context.User.FindFirst(JwtClaimTypes.UserIdClaimType)?.Value ?? string.Empty;
            context.Items[UserContextKeys.UserName] = context.User.Identity?.Name ?? string.Empty;
            context.Items[UserContextKeys.Email] = context.User.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty;
            context.Items[UserContextKeys.Role] = EnumParseHelper.ParseEnumClaim(context.User, ClaimTypes.Role, Role.User);
            context.Items[UserContextKeys.UserStatus] = EnumParseHelper.ParseEnumClaim(context.User, JwtClaimTypes.UserStatusClaimType, UserStatus.Normal);
            context.Items[UserContextKeys.IpAddress] = context.Connection.RemoteIpAddress?.ToString()
                                                       ?? context.Request.Headers[HeaderConstants.ForwardedForHeaderName].FirstOrDefault()
                                                       ?? string.Empty;
        }

        await next(context);
    }
}
