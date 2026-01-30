namespace Infrastructure.Logging.Models;

internal class TelemetrySettings
{
    public bool IsEnabled { get; init; }
    public string? Endpoint { get; init; }
    public string? ApiKey { get; init; }
}
