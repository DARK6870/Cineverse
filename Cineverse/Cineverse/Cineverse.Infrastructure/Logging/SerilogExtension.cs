using Cineverse.Infrastructure.Telemetry;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Cineverse.Infrastructure.Logging;

public static class SerilogExtension
{
    private const string LoggerConfigurationFileName = "logsettings.json";

    public static WebApplicationBuilder AddSerilogLoggingWithOpenTelemetry(
        this WebApplicationBuilder builder,
        IConfiguration configuration
    )
    {
        var cfg = new ConfigurationBuilder()
            .AddJsonFile(LoggerConfigurationFileName, optional: false, reloadOnChange: true)
            .Build();

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(cfg)
            .Enrich.FromLogContext()
            .AddOpenTelemetry(configuration)
            .CreateLogger();

        builder.Host.UseSerilog();

        return builder;
    }
}