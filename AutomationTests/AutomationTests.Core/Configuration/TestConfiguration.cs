using AutomationTests.Core.Configuration.Options;
using Microsoft.Extensions.Configuration;

namespace AutomationTests.Core.Configuration;

public static class TestConfiguration
{
    private static readonly Lazy<IConfigurationRoot> Configuration = new(() =>
        new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
            .Build());

    private static readonly Lazy<CineverseOptions> CineverseOptionsLazy = new(() =>
    {
        var options = Configuration.Value
            .GetRequiredSection(nameof(CineverseOptions))
            .Get<CineverseOptions>()
            ?? throw new NullReferenceException(nameof(CineverseOptions));

        return options;
    });

    public static CineverseOptions Cineverse => CineverseOptionsLazy.Value;
}
