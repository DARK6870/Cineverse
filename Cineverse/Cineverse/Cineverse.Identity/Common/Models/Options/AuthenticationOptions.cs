namespace Cineverse.Identity.Common.Models.Options;

public class AuthenticationOptions
{
    public bool EnableSecurity { get; init; }
    
    public required JwtOptions JwtOptions { get; init; }
}