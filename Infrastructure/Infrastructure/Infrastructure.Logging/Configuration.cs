using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Infrastructure.Logging;

public static class Configuration
{
    private const string EmbeddedConfigResourceName = "logsettings.json";
    
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

        Log.Logger = loggerConfiguration.CreateLogger();

        builder.Host.UseSerilog();

        return builder;
    }
    
    public static WebApplicationBuilder AddInfrastructureLogging(
        this WebApplicationBuilder builder,
        string configFilePath
    )
    {
        if (!File.Exists(configFilePath))
        {
            throw new FileNotFoundException($"Log configuration file not found: {configFilePath}");
        }

        var cfg = new ConfigurationBuilder()
            .AddJsonFile(configFilePath, optional: false, reloadOnChange: true)
            .Build();

        var loggerConfiguration = new LoggerConfiguration()
            .ReadFrom.Configuration(cfg)
            .Enrich.FromLogContext();

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
}