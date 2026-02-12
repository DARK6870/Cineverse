using System.Net.Http.Headers;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.SystemTextJson;
using Infrastructure.Common.Json.Configuration;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace Cineverse.IntegrationTests.Shared.Extensions;

public static class WebApplicationFactoryExtensions
{
    public static GraphQLHttpClient CreateGraphQlHttpClient(
        this WebApplicationFactory<Program> factory,
        string? accessToken = null
    )
    {
        var httpClient = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            HandleCookies = true
        });
        
        if (accessToken is not null)
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        
        var graphQlHttpClient = new GraphQLHttpClient(
            new GraphQLHttpClientOptions
            {
                EndPoint = new Uri("http://localhost/api/graphql")
            },
            new SystemTextJsonSerializer(JsonSerializerConfiguration.GetDefault()),
            httpClient
        );

        return graphQlHttpClient;
    }

    public static T GetRequiredService<T>(
        this WebApplicationFactory<Program> factory
    ) where T : notnull
    {
        return factory.Services.GetRequiredService<T>();
    }
    
    public static T GetRequiredScopedService<T>(
        this WebApplicationFactory<Program> factory
    ) where T : notnull
    {
        using var scope = factory.Services.CreateScope();
        return scope.ServiceProvider.GetRequiredService<T>();
    }
}