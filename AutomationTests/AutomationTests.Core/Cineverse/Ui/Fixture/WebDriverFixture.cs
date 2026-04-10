using AutomationTests.Core.Cineverse.Ui.Pages.Identity;
using AutomationTests.Core.Common.Configuration;
using AutomationTests.Core.Common.Helpers;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Xunit;

namespace AutomationTests.Core.Cineverse.Ui.Fixture;

public sealed class WebDriverFixture : IAsyncLifetime
{
    public IWebDriver Driver { get; private set; } = null!;
    public WebDriverWait Wait { get; private set; } = null!;

    public ValueTask InitializeAsync()
    {
        Driver = WebDriverFactory.Create();
        Wait   = new WebDriverWait(Driver, TimeSpan.FromSeconds(TestConfiguration.Ui.ExplicitWaitSeconds));

        var loginPage = new LoginPage(Driver, Wait);
        loginPage.NavigateTo();
        loginPage.LoginAs("admin@cineverse.com", "TestPass_1");

        // Wait until the browser has navigated away from the login page
        Wait.Until(d => !d.Url.Contains(
            TestConfiguration.Cineverse.IdentityUiPath,
            StringComparison.OrdinalIgnoreCase));

        return ValueTask.CompletedTask;
    }

    public ValueTask DisposeAsync()
    {
        Driver.Quit();
        Driver.Dispose();
        return ValueTask.CompletedTask;
    }
}
