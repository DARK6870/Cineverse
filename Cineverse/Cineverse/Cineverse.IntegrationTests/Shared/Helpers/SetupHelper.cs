using Auth.Models.Enums;
using Cineverse.IntegrationTests.Core.Factory;
using Cineverse.IntegrationTests.Shared.Constants.Test;
using Infrastructure.Context.UserContext;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;

namespace Cineverse.IntegrationTests.Shared.Helpers;

public static class SetupHelper
{
    public static void SetupEnvironmentalVariables(
        this IHostBuilder hostBuilder,
        CineverseWebApplicationFactory factory
    )
    {
        Environment.SetEnvironmentVariable("MongoSettings__ConnectionString", factory.MongoDbConnectionString);
        Environment.SetEnvironmentVariable("AuthenticationOptions__EnableSecurity", bool.FalseString);
    }

    public static void SetupMockServices(this IHostBuilder builder)
    {
        var mockUserContext = new Mock<IUserContext>();
        mockUserContext.SetupGet(x => x.UserId).Returns(TestConstants.TestUserId);
        mockUserContext.SetupGet(x => x.Role).Returns(Role.User);
        mockUserContext.SetupGet(x => x.UserStatus).Returns(UserStatus.Normal);

        builder.ConfigureServices(services =>
        {
            services.AddSingleton(mockUserContext.Object);
        });
    }
}