using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Cineverse.Infrastructure.Common.Models.Options;
using Cineverse.Infrastructure.Services.Interfaces;
using Cineverse.Mongo.Schemas.Enums;
using Microsoft.IdentityModel.Tokens;

namespace Cineverse.Infrastructure.Services.Implementations;

public class AuthenticationService(
    AuthenticationOptions authenticationOptions
) : IAuthenticationService
{
    public string GenerateJwtToken(
        string email,
        Role role,
        string fullName
    )
    {
        var jwtOptions = authenticationOptions.JwtOptions;
        
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(jwtOptions.SecretKey);

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, fullName),
            new(ClaimTypes.Email, email),
            new(ClaimTypes.Role, role.ToString())
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(jwtOptions.ExpireInMinutes),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature
            ),
            Issuer = jwtOptions.Issuer,
            Audience = jwtOptions.Audience
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        const int tokenSize = 32;

        using var rng = RandomNumberGenerator.Create();
        var randomBytes = new byte[tokenSize];
        rng.GetBytes(randomBytes);
        
        return Convert.ToBase64String(randomBytes);
    }
}