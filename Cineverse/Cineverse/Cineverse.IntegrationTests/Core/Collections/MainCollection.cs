using Cineverse.IntegrationTests.Core.Factory;

namespace Cineverse.IntegrationTests.Core.Collections;

[CollectionDefinition(nameof(MainCollection))]
public class MainCollection : ICollectionFixture<CineverseWebApplicationFactory>;