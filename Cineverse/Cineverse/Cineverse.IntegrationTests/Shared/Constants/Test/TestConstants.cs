using MongoDB.Bson;

namespace Cineverse.IntegrationTests.Shared.Constants.Test;

public static class TestConstants
{
    public static readonly string TestUserId = ObjectId.GenerateNewId().ToString();
}