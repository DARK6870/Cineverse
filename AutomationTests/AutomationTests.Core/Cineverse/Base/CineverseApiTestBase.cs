using AutomationTests.Core.Cineverse.Fixture;
using AutomationTests.Core.Common.Constants.Shared;
using Xunit;
using CineverseClient = AutomationTests.Core.Cineverse.Client.CineverseClient;

namespace AutomationTests.Core.Cineverse.Base;

[Collection("CineverseCollection")]
[Trait(Categories.FilterName, Categories.ApiTest)]
public abstract class CineverseApiTestBase(CineverseFixture fixture)
{
    protected readonly CineverseClient AdminClient = fixture.AdminClient;
    protected readonly CineverseClient UserClient = fixture.UserClient;
    protected readonly CineverseClient UnauthorizedClient = fixture.UnauthorizedClient;
}