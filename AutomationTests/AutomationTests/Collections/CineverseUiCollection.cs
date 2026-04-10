using AutomationTests.Core.Cineverse.Ui.Fixture;
using Xunit;

namespace AutomationTests.Collections;

[CollectionDefinition(nameof(CineverseUiCollection), DisableParallelization = true)]
public class CineverseUiCollection : ICollectionFixture<CineverseUiFixture>;
