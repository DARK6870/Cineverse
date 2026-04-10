using AutomationTests.Core.Cineverse.Api.Fixture;
using Xunit;

namespace AutomationTests.Collections;

[CollectionDefinition(nameof(CineverseCollection))]
public class CineverseCollection : ICollectionFixture<CineverseFixture>;