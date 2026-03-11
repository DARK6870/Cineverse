using AutomationTests.Core.Clients.Cineverse;
using AutomationTests.Core.Common.Constants.Shared;
using AutomationTests.Core.Fixtures;
using Xunit;

namespace AutomationTests.Core.TestBases;

[Collection("CineverseCollection")]
[Trait(Categories.FilterName, Categories.ApiTest)]
public abstract class CineverseApiTestBase(CineverseFixture fixture)
{
    protected readonly CineverseClient Client = new(fixture.RestClient, fixture.GraphQlClient);
}