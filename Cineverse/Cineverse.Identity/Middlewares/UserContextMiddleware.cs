using System.Security.Claims;
using Cineverse.Identity.Authentication;
using Cineverse.Identity.Common.Constants;
using Cineverse.Identity.Services.UserContext;
using Cineverse.Mongo.Schemas.Enums;
using Microsoft.AspNetCore.Http;

namespace Cineverse.Identity.Middlewares;

public class UserContextMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, IUserContext userContext)
    {
        var claimsUser = context.User;
        
        
        if (claimsUser.Identity?.IsAuthenticated is true && AuthenticationSetup.EnableSecurity)
        {
            if (userContext is UserContext ctx)
            {
                // Extract User Id
                ctx.UserId = claimsUser.FindFirst(JwtClaims.UserIdClaimType)?.Value ?? string.Empty;
                
                // Extract User Status
                var statusValue = claimsUser.FindFirst(JwtClaims.UserStatusClaimType)?.Value;
                if (statusValue != null && Enum.TryParse<UserStatus>(statusValue, out var status))
                    ctx.UserStatus = status;

                // Extract Username
                ctx.UserName = claimsUser.Identity?.Name ?? string.Empty;
                
                // Extract Email
                ctx.Email = claimsUser.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty;

                // Extract role
                var roleValue = claimsUser.FindFirst(ClaimTypes.Role)?.Value;
                if (roleValue != null && Enum.TryParse<Role>(roleValue, out var role))
                    ctx.Role = role;
                
                // Extract Ip
                var ip = context.Connection.RemoteIpAddress?.ToString() 
                         ?? context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
                
                ctx.IpAddress = ip ?? string.Empty;
            }
        }

        await next(context);
    }
}