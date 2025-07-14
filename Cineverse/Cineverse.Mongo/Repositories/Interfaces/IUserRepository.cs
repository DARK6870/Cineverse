using Cineverse.Mongo.Repositories.Generic;
using Cineverse.Mongo.Schemas.Entities;

namespace Cineverse.Mongo.Repositories.Interfaces;

public interface IUserRepository : IGenericRepository<UserEntity>
{
    Task<UserEntity?> GetUserByCredentialsAsync(string email, string password);

    Task CreateUserAsync(UserEntity user, string password);
}