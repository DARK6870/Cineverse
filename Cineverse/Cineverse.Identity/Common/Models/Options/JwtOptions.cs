namespace Cineverse.Identity.Common.Models.Options;

public class JwtOptions
{
    public required string SecretKey { get; set; }
    
    public required string Issuer { get; set; }
    
    public required string Audience { get; set; }
    public required int ExpireInMinutes { get; set; }
}