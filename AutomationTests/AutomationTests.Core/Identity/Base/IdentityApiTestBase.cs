using AutomationTests.Core.Common.Constants.Shared;
using AutomationTests.Core.Identity.Client;
using AutomationTests.Core.Identity.Fixture;
using Xunit;

namespace AutomationTests.Core.Identity.Base;

[Collection("IdentityCollection")]
[Trait(Categories.FilterName, Categories.IdentityApiTests)]
public abstract class IdentityApiTestBase(IdentityFixture fixture)
{
    protected readonly IdentityClient AdminClient = fixture.AdminClient;
    protected readonly IdentityClient UserClient = fixture.UserClient;
    protected readonly IdentityClient UnauthorizedClient = fixture.UnauthorizedClient;
}