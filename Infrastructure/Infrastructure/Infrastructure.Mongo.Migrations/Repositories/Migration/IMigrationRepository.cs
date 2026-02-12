using Infrastructure.Mongo.Migrations.Entities;
using Infrastructure.Mongo.Repositories.Interfaces.Generic;

namespace Infrastructure.Mongo.Migrations.Repositories.Migration;

public interface IMigrationRepository : IGenericRepository<MigrationEntity>
{
    
}