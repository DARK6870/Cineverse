using AutomationTests.Core.Identity.Fixture;
using Xunit;

namespace AutomationTests.Collections;

[CollectionDefinition(nameof(IdentityCollection))]
public class IdentityCollection : ICollectionFixture<IdentityFixture>;
