using AutomationTests.Core.Common.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;

namespace AutomationTests.Core.Common.Helpers;

public static class WebDriverFactory
{
    public static IWebDriver Create()
    {
        var options = TestConfiguration.Ui;

        var driver = options.BrowserType.ToUpperInvariant() switch
        {
            "CHROME"  => CreateChrome(options.Headless),
            "BRAVE"   => CreateBrave(options.Headless),
            "FIREFOX" => CreateFirefox(options.Headless),
            "EDGE"    => CreateEdge(options.Headless),
            _         => throw new InvalidOperationException($"Unsupported browser type: '{options.BrowserType}'. Valid values: Chrome, Brave, Firefox, Edge.")
        };

        driver.Manage().Timeouts().PageLoad   = TimeSpan.FromSeconds(options.PageLoadTimeoutSeconds);
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.Zero; // always use explicit waits
        driver.Manage().Window.Size = new System.Drawing.Size(1920, 1080);

        return driver;
    }

    private static IWebDriver CreateChrome(bool headless)
    {
        var options = new ChromeOptions();
        if (headless) options.AddArgument("--headless=new");
        options.AddArguments("--no-sandbox", "--disable-dev-shm-usage", "--disable-gpu");
        return new ChromeDriver(options);
    }

    private static IWebDriver CreateBrave(bool headless)
    {
        var options = new ChromeOptions();
        options.BinaryLocation = "/Applications/Brave Browser.app/Contents/MacOS/Brave Browser";
        if (headless) options.AddArgument("--headless=new");
        options.AddArguments("--no-sandbox", "--disable-dev-shm-usage", "--disable-gpu");
        return new ChromeDriver(options);
    }

    private static IWebDriver CreateFirefox(bool headless)
    {
        var options = new FirefoxOptions();
        if (headless) options.AddArgument("--headless");
        return new FirefoxDriver(options);
    }

    private static IWebDriver CreateEdge(bool headless)
    {
        var options = new EdgeOptions();
        if (headless) options.AddArgument("--headless=new");
        options.AddArguments("--no-sandbox", "--disable-dev-shm-usage");
        return new EdgeDriver(options);
    }
}
