using System.Net.Http.Headers;
using AutomationTests.Core.Common.Constants.Cineverse;
using AutomationTests.Core.Common.Extensions;
using AutomationTests.Core.DataGenerators.Cineverse;
using AutomationTests.Models.Cineverse.Entities;
using AutomationTests.Models.Cineverse.Requests.Movie;
using AutomationTests.Models.Generic;
using GraphQL.Client.Abstractions;
using GraphQL.Client.Http;
using Xunit;

namespace AutomationTests.Core.Clients.Cineverse;

public partial class CineverseClient
{
    public async Task<GraphQlPaginatedResponse<MovieEntity[]>> GetMovies()
    {
        var request = new GraphQLHttpRequest
        {
            Query = MovieGraphQlConstants.GetMoviesQuery
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
            Query = MovieGraphQlConstants.GetMovieByIdQuery,
            Variables = new { id }
        };
        
        return await graphQlClient.SendAsync<MovieEntity>(
            request,
            TestContext.Current.CancellationToken
        );
    }

    public async Task<BaseResponse<string>> CreateMovie(CreateMovieRequest? movie = null, string? accessToken = null)
    {
        var request = new GraphQLHttpRequest
        {
            Query = MovieGraphQlConstants.CreateMovieMutation,
            Variables = new { request = movie ?? MovieDataGenerator.ValidCreateMovieRequest() }
        };

        if (!string.IsNullOrWhiteSpace(accessToken))
        {
            graphQlClient.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }

        return await graphQlClient.SendAsync<string>(request, TestContext.Current.CancellationToken);
    }
}
