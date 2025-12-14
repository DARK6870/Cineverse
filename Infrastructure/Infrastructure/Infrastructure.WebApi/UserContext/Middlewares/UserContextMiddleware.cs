using System.Security.Claims;
using Infrastructure.WebApi.UserContext.UserContext;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.WebApi.UserContext.Middlewares;

public class UserContextMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, IUserContext userContext)
    {
        if (context.User.Identity?.IsAuthenticated is true)
        {
            if (userContext is UserContext.UserContext ctx)
            {
                var claimsUser = context.User;

                ctx.UserId = claimsUser.FindFirst("user_id")?.Value ?? string.Empty;
                ctx.UserStatus = claimsUser.FindFirst("user_status")?.Value ?? string.Empty;
                ctx.UserName = claimsUser.Identity?.Name ?? string.Empty;
                ctx.Email = claimsUser.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty;
                ctx.Role = claimsUser.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;

                ctx.IpAddress = context.Connection.RemoteIpAddress?.ToString()
                                ?? context.Request.Headers["X-Forwarded-For"].FirstOrDefault()
                                ?? string.Empty;
            }
        }

        await next(context);
    }
}