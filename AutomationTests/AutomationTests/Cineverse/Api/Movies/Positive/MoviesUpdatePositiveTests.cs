using AutomationTests.Core.Cineverse.Api.Base;
using AutomationTests.Core.Cineverse.Api.DataGenerators;
using AutomationTests.Core.Cineverse.Api.Fixture;
using Xunit;

namespace AutomationTests.Cineverse.Api.Movies.Positive;

public class MoviesUpdatePositiveTests(CineverseFixture fixture) : CineverseApiTestBase(fixture)
{
    [Fact]
    public async Task UpdateMovie_ShouldUpdateMovie()
    {
        // Arrange
        var createMovieResponse = await AdminClient.CreateMovie();
        Assert.True(createMovieResponse.IsSuccess);
        Assert.NotNull(createMovieResponse.Data);
        
        var updateMovieRequest = MovieDataGenerator.ValidUpdateMovieRequest(createMovieResponse.Data);
        
        // Act
        var updateMovieResponse = await AdminClient.UpdateMovie(updateMovieRequest);
        var getMovieByIdResponse = await UnauthorizedClient.GetMovie(createMovieResponse.Data);
        var updatedMovie = getMovieByIdResponse.Data;

        // Assert
        Assert.True(updateMovieResponse.IsSuccess);
        Assert.True(getMovieByIdResponse.IsSuccess);
        
        Assert.NotNull(updatedMovie);
        Assert.Equal(updatedMovie.Title, updateMovieRequest.Title);
        Assert.Equal(updatedMovie.Description, updateMovieRequest.Description);
        Assert.Equal(updatedMovie.Genre, updateMovieRequest.Genre);
        Assert.Equal(updatedMovie.PosterUrl, updateMovieRequest.PosterUrl);
        Assert.Equal(updatedMovie.TrailerUrl, updateMovieRequest.TrailerUrl);
        Assert.Equal(updatedMovie.ReleaseDate, updateMovieRequest.ReleaseDate);
        Assert.Equal(updatedMovie.Duration, updateMovieRequest.Duration);
        Assert.True(updatedMovie.DateModified > updatedMovie.DateCreated);
    }
}