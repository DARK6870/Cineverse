namespace Cineverse.Identity.Common.Models.Options;

public class JwtOptions
{
    public required string SecretKey { get; init; }
    
    public required string Issuer { get; init; }
    
    public required string Audience { get; init; }
    public required int ExpireInMinutes { get; init; }
}