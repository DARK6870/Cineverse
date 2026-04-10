using AutomationTests.Core.Common.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace AutomationTests.Core.Cineverse.Ui.Pages;

public abstract class BasePage
{
    protected readonly IWebDriver Driver;
    protected readonly WebDriverWait Wait;
    protected readonly string BaseUrl;

    protected BasePage(IWebDriver driver, WebDriverWait wait)
    {
        Driver  = driver;
        Wait    = wait;
        BaseUrl = TestConfiguration.Cineverse.HomepageUrl;
    }

    protected abstract string RelativePath { get; }

    /// <summary>
    /// When the server returns 500 for SPA sub-routes, override this with
    /// the app's bootstrap path (e.g. "/admin/"). NavigateTo will load that
    /// path first to bootstrap Angular, then push the real route client-side.
    /// </summary>
    protected virtual string? SpaBootstrapPath => null;

    public string CurrentUrl => Driver.Url;

    public void NavigateTo()
    {
        if (SpaBootstrapPath is not null)
            NavigateViaSpa();
        else
            Driver.Navigate().GoToUrl($"{BaseUrl}{RelativePath}");

        WaitForPageReady();
    }

    protected abstract void WaitForPageReady();

    private void NavigateViaSpa()
    {
        var bootstrapUrl = $"{BaseUrl}{SpaBootstrapPath}";

        if (!Driver.Url.StartsWith(bootstrapUrl, StringComparison.OrdinalIgnoreCase))
        {
            Driver.Navigate().GoToUrl(bootstrapUrl);
            WaitForAngularBootstrap();
        }

        var js = (IJavaScriptExecutor)Driver;
        js.ExecuteScript(
            "window.history.pushState({}, '', arguments[0]);" +
            "window.dispatchEvent(new PopStateEvent('popstate', { state: {} }));",
            $"{BaseUrl}{RelativePath}");
    }

    private void WaitForAngularBootstrap()
    {
        Wait.Until(d => (bool)((IJavaScriptExecutor)d).ExecuteScript(
            "return document.querySelector('app-root') !== null " +
            "&& document.querySelector('app-root').children.length > 0;"));
    }

    protected IWebElement WaitForVisible(By locator)
    {
        return Wait.Until(d =>
        {
            var el = d.FindElement(locator);
            return el.Displayed ? el : null;
        })!;
    }

    protected IReadOnlyList<IWebElement> WaitForAllVisible(By locator)
    {
        Wait.Until(d => d.FindElements(locator).Count > 0);
        return Driver.FindElements(locator);
    }

    protected void WaitForUrlToContain(string fragment)
    {
        Wait.Until(d => d.Url.Contains(fragment, StringComparison.OrdinalIgnoreCase));
    }

    protected void WaitForElementToDisappear(By locator)
    {
        Wait.Until(d =>
        {
            try   { return !d.FindElement(locator).Displayed; }
            catch (NoSuchElementException) { return true; }
        });
    }

    protected bool IsVisible(By locator)
    {
        try   { return Driver.FindElement(locator).Displayed; }
        catch (NoSuchElementException) { return false; }
    }

    protected void Click(By locator) => WaitForVisible(locator).Click();

    protected void Type(By locator, string text)
    {
        var el = WaitForVisible(locator);
        el.Clear();
        el.SendKeys(text);
    }

    protected string GetText(By locator) => WaitForVisible(locator).Text;
}
