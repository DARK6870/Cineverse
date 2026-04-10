using AutomationTests.Core.Cineverse.Ui.Fixture;
using AutomationTests.Core.Cineverse.Ui.Helpers;
using AutomationTests.Core.Cineverse.Ui.Pages.Admin;
using AutomationTests.Core.Common.Constants.Shared;
using AutomationTests.Models.Cineverse.Entities;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Xunit;
using CineverseClient = AutomationTests.Core.Cineverse.Api.Client.CineverseClient;

namespace AutomationTests.Core.Cineverse.Ui.Base;

[Collection("CineverseUiCollection")]
[Trait(Categories.FilterName, Categories.UiTest)]
public abstract class CineverseUiTestBase(CineverseUiFixture fixture)
{
    protected readonly IWebDriver Driver      = fixture.Driver;
    protected readonly WebDriverWait Wait     = fixture.Wait;

    protected readonly CineverseClient AdminClient        = fixture.AdminClient;
    protected readonly CineverseClient UserClient         = fixture.UserClient;
    protected readonly CineverseClient UnauthorizedClient = fixture.UnauthorizedClient;
    protected readonly HallEntity DefaultHall             = fixture.DefaultHall;

    protected readonly IReadOnlyList<SeededMovie> SeededMovies = fixture.SeededMovies;

    protected MoviesPage MoviesPage => new(Driver, Wait);
}
