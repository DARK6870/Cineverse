using AutomationTests.Core.Cineverse.Api.Fixture;
using AutomationTests.Core.Common.Constants.Shared;
using Xunit;
using CineverseClient = AutomationTests.Core.Cineverse.Api.Client.CineverseClient;

namespace AutomationTests.Core.Cineverse.Api.Base;

[Collection("CineverseCollection")]
[Trait(Categories.FilterName, Categories.CineverseApiTests)]
public abstract class CineverseApiTestBase(CineverseFixture fixture)
{
    protected readonly CineverseClient AdminClient = fixture.AdminClient;
    protected readonly CineverseClient UserClient = fixture.UserClient;
    protected readonly CineverseClient UnauthorizedClient = fixture.UnauthorizedClient;
}