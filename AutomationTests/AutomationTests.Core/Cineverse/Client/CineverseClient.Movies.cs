using AutomationTests.Core.Cineverse.Constants;
using AutomationTests.Core.Cineverse.DataGenerators;
using AutomationTests.Core.Common.Extensions;
using AutomationTests.Models.Cineverse.Entities;
using AutomationTests.Models.Cineverse.Requests.Movie;
using AutomationTests.Models.Generic;
using GraphQL.Client.Http;
using Xunit;

namespace AutomationTests.Core.Cineverse.Client;

public partial class CineverseClient
{
    public async Task<GraphQlPaginatedResponse<MovieEntity[]>> GetMovies()
    {
        var request = new GraphQLHttpRequest
        {
            Query = MovieConstants.GetMoviesQuery
        };
        
        var response = await graphQlClient.SendAsync<GraphQlPaginatedResponse<MovieEntity[]>>(
            request,
            TestContext.Current.CancellationToken
        );
        
        return response.Data ?? throw new InvalidOperationException("Movies is null");
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

    public async Task<BaseResponse<string[]>> GetGenreDistinctFilterValues(string genre)
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
