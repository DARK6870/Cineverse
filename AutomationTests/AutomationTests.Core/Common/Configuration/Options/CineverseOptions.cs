namespace AutomationTests.Core.Common.Configuration.Options;

public sealed class CineverseOptions
{
    public required string HomepageUrl { get; init; }
    public required string CineverseApiBasePath { get; init; }
    public required string IdentityApiBasePath { get; init; }
    public required string IdentityUiPath { get; init; }
    public required string AdminUiPath { get; init; }
}
