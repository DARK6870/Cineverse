namespace Infrastructure.WebApi.Cors.Options;

public class CorsOptions
{
    public bool UseCorsPolicy { get; init; } = false;
    
    public required string[] AllowedOrigins { get; init; }
}