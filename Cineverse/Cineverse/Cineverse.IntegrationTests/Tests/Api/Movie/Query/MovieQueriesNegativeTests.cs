using Cineverse.IntegrationTests.Core.Categories;
using Cineverse.IntegrationTests.Core.Collections;
using Cineverse.IntegrationTests.Core.Factory;
using Cineverse.IntegrationTests.Shared.Constants.GraphQl;
using Cineverse.IntegrationTests.Shared.Extensions;
using GraphQL;
using GraphQL.Client.Http;
using MongoDB.Bson;

namespace Cineverse.IntegrationTests.Tests.Api.Movie.Query;

[Collection(nameof(MainCollection))]
[Trait("Category", Categories.IntegrationTests)]
public class MovieQueriesNegativeTests(CineverseWebApplicationFactory factory)
{
    private readonly GraphQLHttpClient _client = factory.CreateGraphQlHttpClient();
    
    [Fact]
    public async Task GetMovieById_ShouldReturnError_WhenMovieDoesNotExists()
    {
        // Arrange
        var request = new GraphQLRequest
        {
            Query = MovieGraphQlConstants.GetMovieByIdQuery,
            Variables = new { id = ObjectId.GenerateNewId().ToString() }
        };

        // Act
        var error = await _client.SendQueryAndExtractErrorAsync(request,  TestContext.Current.CancellationToken);
        
        // Assert
        Assert.NotEmpty(error);
        Assert.Equal("Movie was not found", error);
    }
}