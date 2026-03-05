namespace IdentityService.Application.Common.Models.Options;

public class ProviderConfig
{
    public required string ClientId { get; init; }
    public required string ClientSecret { get; init; }
}