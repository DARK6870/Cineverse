namespace IdentityService.Application.Models.Options;

public class ExternalProvidersOptions
{
    public required string RedirectUrl { get; set; }
    public required ProviderConfig Google { get; set; }
    public required ProviderConfig GitHub { get; set; }
}