using AutomationTests.Core.Cineverse.Ui.Fixture;
using Xunit;

namespace AutomationTests.Collections;

[CollectionDefinition(nameof(UiCollection), DisableParallelization = true)]
public class UiCollection : ICollectionFixture<WebDriverFixture>;
