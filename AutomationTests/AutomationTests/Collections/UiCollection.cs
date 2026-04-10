using AutomationTests.Core.Cineverse.Ui.Fixture;
using Xunit;

namespace AutomationTests.Collections;

[CollectionDefinition(nameof(UiCollection))]
public class UiCollection : ICollectionFixture<WebDriverFixture>;
