using IdentityService.Mongo.Schemas.Entities;

namespace IdentityService.Application.Services.Token;

public interface ITokenService
{
    /// <summary>
    /// Generate access token based on user information
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    string GenerateJwtToken(UserEntity user);
}