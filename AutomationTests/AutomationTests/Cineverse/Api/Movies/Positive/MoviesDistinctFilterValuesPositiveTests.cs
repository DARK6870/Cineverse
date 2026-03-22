using AutomationTests.Core.Cineverse.Base;
using AutomationTests.Core.Cineverse.DataGenerators;
using AutomationTests.Core.Cineverse.Fixture;
using Xunit;

namespace AutomationTests.Cineverse.Api.Movies.Positive;

public class MoviesDistinctFilterValuesPositiveTests(CineverseFixture fixture) : CineverseApiTestBase(fixture)
{
    [Fact]
    public async Task GetGenreDistinctFilterValues_ShouldReturnDistinctValues()
    {
        // Arrange
        var createMovieResponse1 = await AdminClient.CreateMovie(MovieDataGenerator.ValidCreateMovieRequest() with { Genre = "genre-1" });
        var createMovieResponse2 = await AdminClient.CreateMovie(MovieDataGenerator.ValidCreateMovieRequest() with { Genre = "genre-2" });
        Assert.True(createMovieResponse1.IsSuccess);
        Assert.NotNull(createMovieResponse1.Data);
        Assert.True(createMovieResponse2.IsSuccess);
        Assert.NotNull(createMovieResponse2.Data);

        // Act
        var getGenreDistinctFilterValuesResponse = await UnauthorizedClient.GetGenreDistinctFilterValues();

        // Assert
        Assert.True(getGenreDistinctFilterValuesResponse.IsSuccess);
        var filterValues = getGenreDistinctFilterValuesResponse.Data;
        Assert.NotNull(filterValues);
        Assert.True(filterValues.Length >= 2);
        Assert.Equal(filterValues, filterValues.Distinct());
    }
}