/*using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Cineverse.Identity.Common.Constants;
using Cineverse.Identity.Common.Models.Options;
using Cineverse.Mongo.Schemas.Entities;
using Microsoft.IdentityModel.Tokens;

namespace Cineverse.Identity.Services.TokenManagament;

internal class TokenManagementService(
    AuthenticationOptions authenticationOptions
) : ITokenManagementService
{
    private readonly JwtOptions _jwtOptions = authenticationOptions.JwtOptions;

    public string GenerateJwtToken(UserEntity user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtOptions.SecretKey);

        var claims = CreateClaims(user);
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_jwtOptions.ExpireInMinutes),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature
            ),
            Issuer = _jwtOptions.Issuer,
            Audience = _jwtOptions.Audience
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    private static List<Claim> CreateClaims(UserEntity user)
    {
        return
        [
            new Claim(JwtClaims.UserIdClaimType, user.Id),
            new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
            new Claim(JwtClaims.UserStatusClaimType, user.Status.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        ];
    }
}*/