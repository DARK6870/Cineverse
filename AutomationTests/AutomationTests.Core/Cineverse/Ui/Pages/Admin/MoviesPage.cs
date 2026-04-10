using AutomationTests.Core.Common.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace AutomationTests.Core.Cineverse.Ui.Pages.Admin;

public sealed class MoviesPage(IWebDriver driver, WebDriverWait wait)
    : BasePage(driver, wait)
{
    protected override string RelativePath =>
        TestConfiguration.Cineverse.AdminUiPath.TrimEnd('/') + "/movies";

    protected override string? SpaBootstrapPath => TestConfiguration.Cineverse.AdminUiPath;

    private static readonly By PageContainer   = By.CssSelector("section.admin-page");
    private static readonly By MovieTable      = By.CssSelector("p-table.movies-table");
    private static readonly By MovieRows       = By.CssSelector("p-table.movies-table tbody tr.border-b");
    private static readonly By CreateButton    = By.CssSelector("a[href*='movies/new']");
    private static readonly By SearchInput     = By.CssSelector("input[placeholder='Search movies']");
    private static readonly By EditButtons     = By.CssSelector("a[aria-label='Edit movie']");
    private static readonly By MoviesNavButton = By.CssSelector("button[aria-label='Movies']");

    protected override void WaitForPageReady()
    {
        WaitForVisible(PageContainer);
        WaitForTableData();
    }

    public bool IsMovieTableVisible => IsVisible(MovieTable);

    public int MovieCount => Driver.FindElements(MovieRows).Count;

    public void NavigateViaSidebar()
    {
        Driver.Navigate().GoToUrl($"{BaseUrl}{TestConfiguration.Cineverse.AdminUiPath}");
        Click(MoviesNavButton);
        WaitForPageReady();
    }

    public bool IsCreateButtonVisible => IsVisible(CreateButton);

    public bool IsSearchInputVisible => IsVisible(SearchInput);

    public void SearchMovies(string query)
    {
        var input = WaitForVisible(SearchInput);
        input.Clear();
        input.SendKeys(query);
        input.SendKeys(Keys.Enter);
    }

    public bool MovieWithTitleExists(string title)
    {
        SearchMovies(title);

        try
        {
            Wait.Until(d => d.FindElements(MovieRows)
                .Any(r => r.Text.Contains(title, StringComparison.OrdinalIgnoreCase)));
            return true;
        }
        catch (WebDriverTimeoutException)
        {
            return false;
        }
    }

    private void WaitForTableData()
    {
        Wait.Until(d => d.FindElements(MovieRows).Count > 0);
    }
}
