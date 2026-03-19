using AutomationTests.Core.Cineverse.Fixture;
using Xunit;

namespace AutomationTests.Collections;

[CollectionDefinition(nameof(CineverseCollection))]
public class CineverseCollection : ICollectionFixture<CineverseFixture>;