using System.Net;
using System.Security.Claims;
using Cineverse.Infrastructure.Authentication;
using Cineverse.Infrastructure.Common.Constants;
using Cineverse.Infrastructure.Common.Exceptions;
using Cineverse.Infrastructure.Services.Interfaces;
using Cineverse.Mongo.Schemas.Enums;
using Microsoft.AspNetCore.Http;

namespace Cineverse.Infrastructure.Services.Implementations;

public class UserContext(
    IHttpContextAccessor httpContextAccessor
) : IUserContext
{
    private readonly ClaimsPrincipal _user = httpContextAccessor.HttpContext?.User
                                             ?? throw new ApiRequestException("You are not logged in.", HttpStatusCode.Unauthorized);

    public string UserId
    {
        get
        {
            return _user.Claims.FirstOrDefault(c => c.Type == JwtClaims.UserIdClaimType)?.Value ?? "";       
        }
    }

    public UserStatus UserStatus
    {
        get
        {
            if (!AuthenticationSetup.EnableSecurity)
                return UserStatus.Normal;
            
            var statusValue = _user.FindFirst(JwtClaims.UserStatusClaimType)?.Value;

            if (statusValue == null || !Enum.TryParse<UserStatus>(statusValue, out var status))
                throw new NullReferenceException("UserStatus not found or invalid.");

            return status;
        }
    }

    public string UserName
    {
        get
        {
            return _user.Identity?.Name ?? "";
        }
    }

    public string Email
    {
        get
        {
            return _user.Claims.FirstOrDefault(c => c.Type is ClaimTypes.Email)?.Value ?? "";
        }
    } 


    public Role Role
    {
        get
        {
            if (!AuthenticationSetup.EnableSecurity)
                return Role.Admin;
            
            var roleValue = _user.FindFirst(ClaimTypes.Role)?.Value;

            if (roleValue == null || !Enum.TryParse<Role>(roleValue, out var role))
                throw new NullReferenceException("Role not found or invalid.");

            return role;
        }
    }

    public string IpAddress
    {
        get
        {
            var ip = httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();

            if (string.IsNullOrWhiteSpace(ip))
                ip = httpContextAccessor.HttpContext?.Request.Headers["X-Forwarded-For"].FirstOrDefault();

            return ip ?? string.Empty;
        }
    }
}