using Cineverse.Application.MediatR.Requests.Movies.CreateMovie;
using Cineverse.Application.MediatR.Requests.Movies.UpdateMovie;
using Cineverse.IntegrationTests.Core.Categories;
using Cineverse.IntegrationTests.Core.Collections;
using Cineverse.IntegrationTests.Core.Factory;
using Cineverse.IntegrationTests.Shared.Constants.GraphQl;
using Cineverse.IntegrationTests.Shared.Extensions;
using GraphQL;
using GraphQL.Client.Http;
using MongoDB.Bson;

namespace Cineverse.IntegrationTests.Tests.Api.Movie.Mutation;

[Collection(nameof(MainCollection))]
[Trait("Category", Categories.IntegrationTests)]
public class MovieMutationsNegativeTests(CineverseWebApplicationFactory factory)
{
    private readonly GraphQLHttpClient _client = factory.CreateGraphQlHttpClient();

    [Fact]
    public async Task CreateMovie_ShouldReturnValidationErrors_WhenRequestIsInvalid()
    {
        // Arrange
        var createMovieRequest = new CreateMovieRequest(
            "",
            "",
            "",
            "https://test.com/poster.png",
            "https://test.com/trailer.mp4",
            new DateOnly(2025, 06, 06),
            30
        );

        var request = new GraphQLRequest
        {
            Query = MovieGraphQlConstants.CreateMovieMutation,
            Variables = new { request = createMovieRequest }
        };
        
        // Act
        var validationErrors = await _client.SendMutationAndExtractValidationErrorsAsync(request, TestContext.Current.CancellationToken);
        
        // Assert
        Assert.NotEmpty(validationErrors);
        Assert.Contains("Title cannot be empty", validationErrors);
        Assert.Contains("Genre cannot be empty", validationErrors);
        Assert.Contains("Description cannot be empty", validationErrors);
        Assert.Contains("Invalid movie duration", validationErrors);
    }

    [Fact]
    public async Task UpdateMovie_ShouldReturnError_WhenMovieDoesNotExist()
    {
        // Arrange
        var updateMoviesRequest = new UpdateMovieRequest(
            ObjectId.GenerateNewId().ToString(),
            "Test movie",
            "Test genre",
            "Test description",
            "https://test.com/poster.png",
            "https://test.com/trailer.mp4",
            new DateOnly(2025, 06, 06),
            120,
            true
        );

        var request = new GraphQLRequest
        {
            Query = MovieGraphQlConstants.UpdateMovieMutation,
            Variables = new { request = updateMoviesRequest }
        };

        // Act
        var error = await _client.SendMutationAndExtractErrorAsync(request, TestContext.Current.CancellationToken);

        // Assert
        Assert.NotEmpty(error);
        Assert.Equal("Movie was not found", error);
    }
}