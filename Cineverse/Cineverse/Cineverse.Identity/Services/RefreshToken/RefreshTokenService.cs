/*using System.Security.Cryptography;
using Cineverse.Identity.Services.UserContext;
using Cineverse.Mongo.Repositories.RefreshToken;
using Cineverse.Mongo.Schemas.Entities;
using MongoDB.Driver.Linq;

namespace Cineverse.Identity.Services.RefreshToken;

public class RefreshTokenService(
    IRefreshTokenRepository refreshTokenRepository
) : IRefreshTokenService
{
    private const int RefreshTokenSize = 32;
    
    public async Task<RefreshTokenEntity?> GetActiveTokenAsync(string userId, string ipAddress)
    {
        return await refreshTokenRepository
            .AsQueryable()
            .FirstOrDefaultAsync(x => 
                x.UserId == userId && 
                x.IpAddress == ipAddress
            );
    }
    
    public async Task<RefreshTokenEntity?> GetRefreshTokenAsync(string refreshToken, string ipAddress)
    {
        return await refreshTokenRepository
            .AsQueryable()
            .FirstOrDefaultAsync(x => 
                x.Token == refreshToken && 
                x.IpAddress == ipAddress
            );
    }

    public async Task<RefreshTokenEntity> CreateOrUpdateTokenAsync(string userId, string ipAddress)
    {
        var existingToken = await GetActiveTokenAsync(userId, ipAddress);

        if (existingToken is not null)
            await refreshTokenRepository.DeleteByIdAsync(existingToken.Id);

        var refreshTokenEntity = new RefreshTokenEntity
        {
            UserId = userId,
            IpAddress = ipAddress,
            Token = GenerateRefreshToken()
        };

        await refreshTokenRepository.InsertOneAsync(refreshTokenEntity);
        return refreshTokenEntity;
    }

    public async Task<bool> ValidateTokenAsync(string refreshToken, string ipAddress)
    {
        return await refreshTokenRepository
            .ExistsAsync(x =>
                x.Token == refreshToken &&
                x.IpAddress == ipAddress
            );
    }

    public async Task RevokeTokenAsync(string refreshToken)
    {
        await refreshTokenRepository.DeleteOneAsync(x => x.Token == refreshToken);
    }

    public string GenerateRefreshToken()
    {
        using var rng = RandomNumberGenerator.Create();
        var randomBytes = new byte[RefreshTokenSize];
        rng.GetBytes(randomBytes);
        
        return Convert.ToBase64String(randomBytes);
    }
}*/