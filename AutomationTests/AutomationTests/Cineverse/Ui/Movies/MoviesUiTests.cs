using AutomationTests.Core.Cineverse.Ui.Base;
using AutomationTests.Core.Cineverse.Ui.Fixture;
using AutomationTests.Core.Common.Configuration;
using Xunit;

namespace AutomationTests.Cineverse.Ui.Movies;

public class MoviesUiTests(CineverseUiFixture fixture) : CineverseUiTestBase(fixture)
{
    private static readonly string ExpectedUrlFragment = TestConfiguration.Cineverse.AdminUiPath.TrimEnd('/') + "/movies";

    [Fact]
    public void MoviesPage_ShouldNavigateToCorrectUrl()
    {
        var page = MoviesPage;

        page.NavigateTo();

        Assert.Contains(ExpectedUrlFragment, page.CurrentUrl);
    }

    [Fact]
    public void MoviesPage_ShouldDisplayMoviesTable()
    {
        var page = MoviesPage;

        page.NavigateTo();

        Assert.True(page.IsMovieTableVisible);
    }

    [Fact]
    public void MoviesPage_ShouldNotShowErrorInTitle()
    {
        var page = MoviesPage;

        page.NavigateTo();

        Assert.DoesNotContain("404", Driver.Title, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("error", Driver.Title, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void MoviesPage_ShouldOpenViaSidebarNavigation()
    {
        var page = MoviesPage;

        page.NavigateViaSidebar();

        Assert.Contains(ExpectedUrlFragment, page.CurrentUrl);
        Assert.True(page.IsMovieTableVisible);
    }

    [Fact]
    public void MoviesPage_ShouldDisplaySeededMovies()
    {
        var page = MoviesPage;

        page.NavigateTo();

        Assert.All(SeededMovies, movie => Assert.True(page.MovieWithTitleExists(movie.Title)));
    }
}
