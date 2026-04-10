using AutomationTests.Core.Cineverse.Api.Base;
using AutomationTests.Core.Cineverse.Api.Fixture;
using Xunit;

namespace AutomationTests.Cineverse.Api.Movies.Positive;

public class MoviesGetPositiveTests(CineverseFixture fixture) : CineverseApiTestBase(fixture)
{
    [Fact]
    public async Task GetMovies_ShouldReturnAllMovies()
    {
        // Arrange
        var createMovieResponse1 = await AdminClient.CreateMovie();
        var createMovieResponse2 = await AdminClient.CreateMovie();
        
        // Act
        var getMoviesResponse = await UnauthorizedClient.GetMovies();

        // Assert
        Assert.True(getMoviesResponse.IsSuccess);
        Assert.NotNull(getMoviesResponse.Data);
        
        var paginatedMovies = getMoviesResponse.Data;
        Assert.NotEmpty(paginatedMovies.Items);
        Assert.False(paginatedMovies.PageInfo.HasPreviousPage);
        Assert.True(paginatedMovies.TotalCount >= 2);
        Assert.True(paginatedMovies.Items.Length >= 2);
        Assert.NotNull(paginatedMovies.Items.FirstOrDefault(x => x.Id == createMovieResponse1.Data));
        Assert.NotNull(paginatedMovies.Items.FirstOrDefault(x => x.Id == createMovieResponse2.Data));
    }

    [Fact]
    public async Task GetMovies_ShouldReturnPaginatedMovies()
    {
        // Arrange
        var createMovieResponse1 = await AdminClient.CreateMovie();
        var createMovieResponse2 = await AdminClient.CreateMovie();
        
        Assert.True(createMovieResponse1.IsSuccess);
        Assert.NotNull(createMovieResponse1.Data);
        Assert.True(createMovieResponse2.IsSuccess);
        Assert.NotNull(createMovieResponse2.Data);
        
        // Act
        var filter = "{ id: { in: " + $"[\"{createMovieResponse1.Data}\", \"{createMovieResponse2.Data}\"]" + " } }";
        var sort = "{ dateCreated: ASC }";
        var getMoviesResponse1 = await UnauthorizedClient.GetMovies(1, 1,filter, sort);
        var getMoviesResponse2 = await UnauthorizedClient.GetMovies(1, 2, filter, sort);

        // Assert
        Assert.True(getMoviesResponse1.IsSuccess);
        Assert.NotNull(getMoviesResponse1.Data);
        Assert.False(getMoviesResponse1.Data.PageInfo.HasPreviousPage);
        Assert.True(getMoviesResponse1.Data.PageInfo.HasNextPage);
        Assert.True(getMoviesResponse1.Data.TotalCount is 2);
        Assert.True(getMoviesResponse1.Data.Items.Length is 1);
        
        Assert.True(getMoviesResponse2.IsSuccess);
        Assert.NotNull(getMoviesResponse2.Data);
        Assert.True(getMoviesResponse2.Data.PageInfo.HasPreviousPage);
        Assert.True(getMoviesResponse2.Data.Items.Length is 1);
        Assert.False(getMoviesResponse2.Data.PageInfo.HasNextPage);
        
        Assert.NotEqual(getMoviesResponse1.Data.Items, getMoviesResponse2.Data.Items);
        Assert.Equal(getMoviesResponse1.Data.Items.First().Id, createMovieResponse1.Data);
        Assert.Equal(getMoviesResponse2.Data.Items.First().Id, createMovieResponse2.Data);
    }
    
    [Fact]
    public async Task GetMovies_WithSorting_ShouldReturnSortedMovies()
    {
        // Arrange
        await AdminClient.CreateMovie();
        await AdminClient.CreateMovie();
        
        // Act
        var getMoviesResponse1 = await UnauthorizedClient.GetMovies(sorting: "{ title: ASC }");
        var getMoviesResponse2 = await UnauthorizedClient.GetMovies(sorting: "{ title: DESC }");

        // Assert
        Assert.True(getMoviesResponse1.IsSuccess);
        Assert.NotNull(getMoviesResponse1.Data);
        
        var movies1 = getMoviesResponse1.Data.Items;
        Assert.Equal(movies1.OrderBy(x => x.Title, StringComparer.Ordinal), movies1);
        
        Assert.True(getMoviesResponse2.IsSuccess);
        Assert.NotNull(getMoviesResponse2.Data);

        var movies2 = getMoviesResponse2.Data.Items;
        Assert.Equal(movies2.OrderByDescending(x => x.Title, StringComparer.Ordinal), movies2);
    }
    
    [Fact]
    public async Task GetMovies_WithFiltering_ShouldReturnFilteredMovies()
    {
        // Arrange
        var createMovieResponse1 = await AdminClient.CreateMovie();
        var createMovieResponse2 = await AdminClient.CreateMovie();
        
        Assert.True(createMovieResponse1.IsSuccess);
        Assert.NotNull(createMovieResponse1.Data);
        Assert.True(createMovieResponse2.IsSuccess);
        Assert.NotNull(createMovieResponse2.Data);
        
        // Act
        var filter = "{ id: { eq: " + $"\"{createMovieResponse1.Data}\"" + " } }";
        var getMoviesResponse = await UnauthorizedClient.GetMovies(1, 1,filter);

        // Assert
        Assert.True(getMoviesResponse.IsSuccess);
        Assert.NotNull(getMoviesResponse.Data);
        Assert.False(getMoviesResponse.Data.PageInfo.HasPreviousPage);
        Assert.False(getMoviesResponse.Data.PageInfo.HasNextPage);
        Assert.True(getMoviesResponse.Data.TotalCount is 1);
        Assert.True(getMoviesResponse.Data.Items.Length is 1);
        Assert.Equal(getMoviesResponse.Data.Items.First().Id, createMovieResponse1.Data);
    }
}