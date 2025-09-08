namespace Cineverse.Infrastructure.Common.Options;

internal class TelemetryOptions
{
    public required string EndpointUrl { get; init; }
    public required string ApiKey { get; init; }
    public required string ServiceName { get; init; }
}