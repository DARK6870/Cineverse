using AutomationTests.Core.Cineverse.Constants;
using AutomationTests.Core.Cineverse.DataGenerators;
using AutomationTests.Core.Common.Constants.Shared;
using AutomationTests.Core.Common.Extensions;
using AutomationTests.Models.Cineverse.Entities;
using AutomationTests.Models.Cineverse.Requests.Movie;
using AutomationTests.Models.Generic;
using GraphQL.Client.Http;
using Xunit;

namespace AutomationTests.Core.Cineverse.Client;

public partial class CineverseClient
{
    public async Task<BaseResponse<GraphQlPaginatedResponse<MovieEntity[]>>> GetMovies(
        int pageSize = 250,
        int pageNumber = 1,
        string filtering = "null",
        string sorting = "null"
    )
    {
        var query = MovieConstants.GetMoviesQuery
            .Replace(GraphQlConstants.FilteringKey, filtering)
            .Replace(GraphQlConstants.SortingKey, sorting);
        
        var request = new GraphQLHttpRequest
        {
            Query = query,
            Variables = new { take = pageSize, skip = (pageNumber-1) * pageSize }
        };
        
        return await graphQlClient.SendAsync<GraphQlPaginatedResponse<MovieEntity[]>>(
            request,
            TestContext.Current.CancellationToken
        );
    }

    public async Task<BaseResponse<MovieEntity>> GetMovie(string id)
    {
        var request = new GraphQLHttpRequest
        {
            Query = MovieConstants.GetMovieByIdQuery,
            Variables = new { id }
        };
        
        return await graphQlClient.SendAsync<MovieEntity>(request, TestContext.Current.CancellationToken);
    }

    public async Task<BaseResponse<string[]>> GetGenreDistinctFilterValues()
    {
        var request = new GraphQLHttpRequest
        {
            Query = MovieConstants.GetGenreDistinctFilterValuesQuery
        };
        
        return await graphQlClient.SendAsync<string[]>(request, TestContext.Current.CancellationToken);
    }

    public async Task<BaseResponse<string>> CreateMovie(CreateMovieRequest? movie = null)
    {
        var request = new GraphQLHttpRequest
        {
            Query = MovieConstants.CreateMovieMutation,
            Variables = new { request = movie ?? MovieDataGenerator.ValidCreateMovieRequest() }
        };

        return await graphQlClient.SendAsync<string>(request, TestContext.Current.CancellationToken);
    }

    public async Task<BaseResponse<bool>> UpdateMovie(UpdateMovieRequest updatedMovie)
    {
        var request = new GraphQLHttpRequest
        {
            Query = MovieConstants.UpdateMovieMutation,
            Variables = new { request = updatedMovie }
        };

        return await graphQlClient.SendAsync<bool>(request, TestContext.Current.CancellationToken);
    }

    public async Task<BaseResponse<bool>> DeleteMovie(string id)
    {
        var request = new GraphQLHttpRequest
        {
            Query = MovieConstants.DeleteMovieMutation,
            Variables = new { id }
        };
        
        return await graphQlClient.SendAsync<bool>(request, TestContext.Current.CancellationToken);
    }
}
