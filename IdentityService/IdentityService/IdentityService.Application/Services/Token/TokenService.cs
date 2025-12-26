using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Auth.Constants;
using Auth.Models.Options;
using IdentityService.Mongo.Schemas.Entities;
using Microsoft.IdentityModel.Tokens;

namespace IdentityService.Application.Services.Token;

public class TokenService(
    AuthenticationOptions authenticationOptions
) : ITokenService
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
            new Claim(JwtClaimTypes.UserIdClaimType, user.Id),
            new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
            new Claim(JwtClaimTypes.UserStatusClaimType, user.Status.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        ];
    }
}