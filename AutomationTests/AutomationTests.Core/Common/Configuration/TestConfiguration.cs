using AutomationTests.Core.Common.Configuration.Options;
using Microsoft.Extensions.Configuration;

namespace AutomationTests.Core.Common.Configuration;

public static class TestConfiguration
{
    private const string FileName = "appsettings.json";
    
    private static readonly Lazy<IConfigurationRoot> Configuration = new(() =>
        new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile(FileName, optional: false, reloadOnChange: false)
            .Build());

    private static readonly Lazy<CineverseOptions> CineverseOptionsLazy = new(() =>
    {
        var options = Configuration.Value
            .GetRequiredSection(nameof(CineverseOptions))
            .Get<CineverseOptions>()
            ?? throw new NullReferenceException(nameof(CineverseOptions));

        return options;
    });
    
    private static readonly Lazy<MongoOptions> MongoOptionsLazy = new(() =>
    {
        var options = Configuration.Value
                          .GetRequiredSection(nameof(MongoOptions))
                          .Get<MongoOptions>()
                      ?? throw new NullReferenceException(nameof(MongoOptions));

        return options;
    });

    private static readonly Lazy<UiOptions> UiOptionsLazy = new(() =>
    {
        var options = Configuration.Value
                          .GetRequiredSection(nameof(UiOptions))
                          .Get<UiOptions>()
                      ?? throw new NullReferenceException(nameof(UiOptions));

        return options;
    });

    private static readonly Lazy<ImapOptions> ImapOptionsLazy = new(() =>
    {
        var options = Configuration.Value
            .GetRequiredSection(nameof(ImapOptions))
            .Get<ImapOptions>() ?? throw new NullReferenceException(nameof(ImapOptions));

        return options;
    });

    public static CineverseOptions Cineverse => CineverseOptionsLazy.Value;
    public static MongoOptions Mongo => MongoOptionsLazy.Value;
    public static UiOptions Ui => UiOptionsLazy.Value;
    public static ImapOptions Imap => ImapOptionsLazy.Value;
}
