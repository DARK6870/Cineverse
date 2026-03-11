using AutomationTests.Core.Fixtures;
using Xunit;

namespace AutomationTests.Collections;

[CollectionDefinition(nameof(CineverseCollection))]
public class CineverseCollection : ICollectionFixture<CineverseFixture>;