using Infrastructure.Mongo.Migrations.Entities;
using Infrastructure.Mongo.Repositories.Implementations;
using MongoDB.Driver;

namespace Infrastructure.Mongo.Migrations.Repositories.Migration;

public class MigrationRepository(
    IMongoDatabase mongoDatabase
): GenericRepository<MigrationEntity>(mongoDatabase), IMigrationRepository;