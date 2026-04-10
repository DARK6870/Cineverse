using AutomationTests.Core.Cineverse.Api.Fixture;
using AutomationTests.Core.Cineverse.Ui.Helpers;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace AutomationTests.Core.Cineverse.Ui.Fixture;

public sealed class CineverseUiFixture : CineverseFixture
{
    private readonly WebDriverFixture _browser = new();

    public IWebDriver Driver  => _browser.Driver;
    public WebDriverWait Wait => _browser.Wait;

    public IReadOnlyList<SeededMovie> SeededMovies { get; private set; } = [];

    public override async ValueTask InitializeAsync()
    {
        await base.InitializeAsync();
        await _browser.InitializeAsync();
        SeededMovies = await CineverseUiFixtureSetupHelper.SeedMoviesWithScreeningsAsync(AdminClient, DefaultHall);
    }

    public override async ValueTask DisposeAsync()
    {
        await _browser.DisposeAsync();
        await base.DisposeAsync();
    }
}
