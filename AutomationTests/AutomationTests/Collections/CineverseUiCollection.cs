using AutomationTests.Core.Cineverse.Ui.Fixture;
using Xunit;

namespace AutomationTests.Collections;

[CollectionDefinition(nameof(CineverseUiCollection))]
public class CineverseUiCollection : ICollectionFixture<CineverseUiFixture>;
