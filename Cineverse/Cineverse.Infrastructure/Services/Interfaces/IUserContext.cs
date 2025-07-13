using Cineverse.Mongo.Schemas.Enums;

namespace Cineverse.Infrastructure.Services.Interfaces;

public interface IUserContext
{
    string UserName { get; }
    
    string Email { get; }
    
    Role Role { get; }
    

    string GetRefreshTokenFromCookie();

    void AddRefreshTokenToCookie(string refreshToken);
    
    void RemoveRefreshTokenFromCookie();
    
}