using System.Reflection;
using Infrastructure.Logging.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Sinks.OpenTelemetry;

namespace Infrastructure.Logging;

public static class Configuration
{
    private const string EmbeddedConfigResourceName = "logsettings.json";
    private const string ServiceNameAttribute = "service.name";
    private const string ApiKeyHeader = "X-Seq-ApiKey";
    
    public static WebApplicationBuilder AddInfrastructureLogging(
        this WebApplicationBuilder builder,
        Action<LoggerConfiguration>? configureLogger = null
    )
    {
        var config = LoadEmbeddedConfiguration();

        var loggerConfiguration = new LoggerConfiguration()
            .ReadFrom.Configuration(config)
            .Enrich.FromLogContext();

        configureLogger?.Invoke(loggerConfiguration);
        loggerConfiguration.AddTelemetry(builder.Configuration);

        Log.Logger = loggerConfiguration.CreateLogger();
        builder.Host.UseSerilog();

        return builder;
    }
    
    private static IConfiguration LoadEmbeddedConfiguration()
    {
        var assembly = Assembly.GetExecutingAssembly();

        var resourceName = assembly
            .GetManifestResourceNames()
            .FirstOrDefault(r => r.EndsWith(EmbeddedConfigResourceName, StringComparison.Ordinal));

        if (resourceName is null)
        {
            throw new InvalidOperationException(
                $"Embedded configuration file '{EmbeddedConfigResourceName}' not found. " +
                "Make sure it is marked as EmbeddedResource in the .csproj file."
            );
        }

        using var stream = assembly.GetManifestResourceStream(resourceName)
                           ?? throw new InvalidOperationException($"Failed to load embedded resource '{resourceName}'.");

        return new ConfigurationBuilder()
            .AddJsonStream(stream)
            .Build();
    }

    private static void AddTelemetry(
        this LoggerConfiguration loggerConfiguration,
        IConfiguration configuration
    )
    {
        var telemetrySettings = configuration
            .GetSection(nameof(TelemetrySettings))
            .Get<TelemetrySettings>();

        if (telemetrySettings is null || !telemetrySettings.IsEnabled)
            return;
        
        loggerConfiguration.WriteTo.OpenTelemetry(options =>
        {
            options.Endpoint = telemetrySettings.Endpoint;
            options.Protocol = OtlpProtocol.HttpProtobuf;
            
            options.Headers = new Dictionary<string, string>
            {
                [ApiKeyHeader] = telemetrySettings.ApiKey!,
            };
            
            options.ResourceAttributes = new Dictionary<string, object>
            {
                [ServiceNameAttribute] = AppDomain.CurrentDomain.FriendlyName
            };
        });
    }
}