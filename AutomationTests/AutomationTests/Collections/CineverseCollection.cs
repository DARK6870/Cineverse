using AutomationTests.Core.Cineverse.Api.Fixture;
using Xunit;

namespace AutomationTests.Collections;

[CollectionDefinition(nameof(CineverseCollection), DisableParallelization = true)]
public class CineverseCollection : ICollectionFixture<CineverseFixture>;