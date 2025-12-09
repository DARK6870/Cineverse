using Cineverse.Mongo.Repositories.Generic;
using Cineverse.Mongo.Schemas.Entities;

namespace Cineverse.Mongo.Repositories.Migration;

public interface IMigrationRepository : IGenericRepository<MigrationEntity>
{
    
}