namespace AutomationTests.Core.Common.Configuration.Options;

public sealed class UiOptions
{
    public required string BrowserType { get; init; }
    public required bool Headless { get; init; }
    public required int ExplicitWaitSeconds { get; init; }
    public required int PageLoadTimeoutSeconds { get; init; }
}
