using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Cineverse.Infrastructure.Logging;

public static class SerilogExtensions
{
    private const string LoggerConfigurationFileName = "logsettings.json";
    
    public static WebApplicationBuilder AddSerilogLogging(
        this WebApplicationBuilder builder)
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile(LoggerConfigurationFileName, optional: false, reloadOnChange: true)
            .Build();

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .Enrich.FromLogContext()
            .CreateLogger();

        builder.Host.UseSerilog();
        
        return builder;
    }
}