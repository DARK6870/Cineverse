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

    public string UserId => _user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "";
    
    public string UserName => _user.Identity?.Name ?? "";

    public string Email => _user.Claims.FirstOrDefault(c => c.Type is ClaimTypes.Email)?.Value ?? "";


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

    public string? GetRefreshTokenFromCookie()
    {
        return httpContextAccessor.HttpContext?.Request.Cookies[CookieConstants.RefreshTokenCookieKey];
    }

    public void AddRefreshTokenToCookie(string refreshToken)
    {
        httpContextAccessor
            .HttpContext?
            .Response
            .Cookies.Append(
                CookieConstants.RefreshTokenCookieKey,
                refreshToken,
                CookieConstants.CookieOptions
            );
    }

    public void RemoveRefreshTokenFromCookie()
    {
        httpContextAccessor
            .HttpContext?
            .Response
            .Cookies
            .Delete(CookieConstants.RefreshTokenCookieKey);
    }
}