using AutomationTests.Core.Common.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace AutomationTests.Core.Cineverse.Ui.Pages.Identity;

public sealed class LoginPage(IWebDriver driver, WebDriverWait wait)
    : BasePage(driver, wait)
{
    protected override string RelativePath =>
        TestConfiguration.Cineverse.IdentityUiPath.TrimEnd('/') + "/login";

    private static readonly By EmailInput   = By.Id("email");
    private static readonly By PasswordInput = By.Id("password");
    private static readonly By SubmitButton  = By.CssSelector("button[type='submit']");
    private static readonly By ErrorMessage  = By.CssSelector("p-message[severity='error']");

    protected override void WaitForPageReady()
    {
        WaitForVisible(EmailInput);
    }

    public void LoginAs(string email, string password)
    {
        Type(EmailInput, email);
        Type(PasswordInput, password);
        Click(SubmitButton);
    }

    public bool HasError() => IsVisible(ErrorMessage);

    public string ErrorText => GetText(ErrorMessage);
}
