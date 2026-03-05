namespace IdentityService.Application.Common.Models.Options;

public class ExternalProvidersOptions
{
    public required string RedirectUrl { get; init; }
    public required ProviderConfig Google { get; init; }
    public required ProviderConfig GitHub { get; init; }
}