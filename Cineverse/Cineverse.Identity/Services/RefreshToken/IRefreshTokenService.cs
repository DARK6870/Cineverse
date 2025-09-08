using Cineverse.Mongo.Schemas.Entities;

namespace Cineverse.Identity.Services.RefreshToken;

public interface IRefreshTokenService
{
    Task<RefreshTokenEntity?> GetActiveTokenAsync(string userId, string ipAddress);
    
    Task<RefreshTokenEntity?> GetRefreshTokenAsync(string refreshToken, string ipAddress);
    
    Task<RefreshTokenEntity> CreateOrUpdateTokenAsync(string userId, string ipAddress);
    
    Task<bool> ValidateTokenAsync(string refreshToken, string ipAddress);
    
    Task RevokeTokenAsync(string refreshToken);
    
    string GenerateRefreshToken();
}