using Cineverse.IntegrationTests.Core.Categories;
using Cineverse.IntegrationTests.Core.Collections;
using Cineverse.IntegrationTests.Core.Factory;
using Cineverse.IntegrationTests.Shared.Constants.GraphQl;
using Cineverse.IntegrationTests.Shared.Extensions;
using Cineverse.IntegrationTests.Shared.Helpers;
using Cineverse.IntegrationTests.Shared.Models;
using Cineverse.Mongo.Schemas.Entities;
using GraphQL;
using GraphQL.Client.Http;
using MongoDB.Bson;

namespace Cineverse.IntegrationTests.Tests.Api.Movie.Query;

[Collection(nameof(MainCollection))]
[Trait("Category", Categories.IntegrationTests)]
public class MovieQueriesPositiveTests(CineverseWebApplicationFactory factory)
{
    private readonly GraphQLHttpClient _client = factory.CreateGraphQlHttpClient();

    [Fact]
    public async Task GetMovies_ShouldReturnMovies_WhenMoviesExists()
    {
        // Arrange
        var movie1 = await factory.GenerateMovieAsync();
        var movie2 = await factory.GenerateMovieAsync();
        
        var request = new GraphQLRequest { Query = MovieGraphQlConstants.GetMoviesQuery };
        
        // Act
        var paginatedResponse = await _client.SendQueryAndExtractDataAsync<GraphQlPaginatedResponse<MovieEntity[]>>(
            request,
            TestContext.Current.CancellationToken
        );

        // Assert
        Assert.NotEmpty(paginatedResponse.Items);
        Assert.True(paginatedResponse.Items.Length > 0);
        Assert.EquivalentWithExclusions(
            paginatedResponse.Items.First(x => x.Id == movie1.Id),
            movie1,
            x => x.DateCreated,
             x => x.DateModified
        );
        Assert.EquivalentWithExclusions(
            paginatedResponse.Items.First(x => x.Id == movie2.Id),
            movie2,
            x => x.DateCreated,
            x => x.DateModified
        );
    }
    
    [Fact]
    public async Task GetMovieById_ShouldReturnMovie_WhenMovieExists()
    {
        // Arrange
        var movie = await factory.GenerateMovieAsync();
        var request = new GraphQLRequest
        {
            Query = MovieGraphQlConstants.GetMovieByIdQuery,
            Variables = new { id = movie.Id }
        };

        // Act
        var movieById = await _client.SendQueryAndExtractDataAsync<MovieEntity>(request,  TestContext.Current.CancellationToken);
        
        // Assert
        Assert.EquivalentWithExclusions(
            movie,
            movieById,
            x => x.DateCreated,
            x => x.DateModified
        );
    }
}