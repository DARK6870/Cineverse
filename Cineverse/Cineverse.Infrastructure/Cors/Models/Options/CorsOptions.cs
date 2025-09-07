namespace Cineverse.Infrastructure.Cors.Models.Options;

public class CorsOptions
{
    public required string[] AllowedOrigins { get; init; }
}