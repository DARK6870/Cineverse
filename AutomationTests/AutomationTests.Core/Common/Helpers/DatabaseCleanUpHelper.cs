using AutomationTests.Core.Common.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;

namespace AutomationTests.Core.Common.Helpers;

public static class DatabaseCleanUpHelper
{
   public static async Task CleanDataFromDatabase(DateTime startDate)
   {
      var mongoClient = new MongoClient(TestConfiguration.Mongo.ConnectionString);
      
      foreach (var databaseName in TestConfiguration.Mongo.DatabaseNames)
      {
         var cineverseDatabase = mongoClient.GetDatabase(databaseName);

         var collections = await cineverseDatabase.ListCollectionNamesAsync();
         var collectionNames = collections.ToList();
      
         foreach (var collectionName in collectionNames)
         {
            var filter = Builders<BsonDocument>.Filter.Gt("dateCreated", startDate);
            var collection = cineverseDatabase.GetCollection<BsonDocument>(collectionName);
            await collection.DeleteManyAsync(filter);
         }
      }
   }
}