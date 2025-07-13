using Cineverse.Mongo.Schemas.Enums;

namespace Cineverse.Infrastructure.Services.Interfaces;

public interface IAuthenticationService
{
    public string GenerateJwtToken(
        string email,
        Role role,
        string fullName
    );
    
    public string GenerateRefreshToken();
}