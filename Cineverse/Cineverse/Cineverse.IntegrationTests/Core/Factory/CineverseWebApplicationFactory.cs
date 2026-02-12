using Cineverse.IntegrationTests.Shared.Helpers;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;
using Testcontainers.MongoDb;

namespace Cineverse.IntegrationTests.Core.Factory;

public sealed class CineverseWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    #region Containers
    
    private readonly MongoDbContainer _mongoContainer =
        new MongoDbBuilder("mongo:8.0")
            .WithUsername("admin")
            .WithPassword("admin")
            .Build();
    
    public string MongoDbConnectionString => _mongoContainer.GetConnectionString();
    
    #endregion
    
    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.SetupEnvironmentalVariables(this);
        builder.SetupMockServices();
        
        return base.CreateHost(builder);
    }
    
    public async ValueTask InitializeAsync()
    {
        await _mongoContainer.StartAsync();
    }
}