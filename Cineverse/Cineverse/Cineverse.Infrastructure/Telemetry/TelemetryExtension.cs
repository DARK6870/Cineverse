using Cineverse.Infrastructure.Common.Options;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Sinks.OpenTelemetry;

namespace Cineverse.Infrastructure.Telemetry;

internal static class TelemetryExtension
{
    private const string ServiceNameAttribute = "service.name";
    private const string ApiKeyHeader = "X-Seq-ApiKey";
    public static LoggerConfiguration AddOpenTelemetry(
        this LoggerConfiguration loggerConfiguration,
        IConfiguration configuration
    )
    {
        var telemetryConfiguration = configuration
            .GetSection(nameof(TelemetryOptions))
            .Get<TelemetryOptions>() ?? throw new ArgumentNullException(nameof(TelemetryOptions));
        
        loggerConfiguration.WriteTo.OpenTelemetry(options =>
        {
            options.Endpoint = telemetryConfiguration.EndpointUrl;
            options.Protocol = OtlpProtocol.HttpProtobuf;
            
            options.Headers = new Dictionary<string, string>
            {
                [ApiKeyHeader] = telemetryConfiguration.ApiKey,
            };
            
            options.ResourceAttributes = new Dictionary<string, object>
            {
                [ServiceNameAttribute] = telemetryConfiguration.ServiceName
            };
        });
        
        return loggerConfiguration;
    }
}