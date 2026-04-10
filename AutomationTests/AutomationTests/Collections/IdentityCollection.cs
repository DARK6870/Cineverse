using AutomationTests.Core.Identity.Fixture;
using Xunit;

namespace AutomationTests.Collections;

[CollectionDefinition(nameof(IdentityCollection), DisableParallelization = true)]
public class IdentityCollection : ICollectionFixture<IdentityFixture>;
