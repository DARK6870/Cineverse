using Cineverse.Mongo.Common.Helpers;
using Cineverse.Mongo.Repositories.Generic;
using Cineverse.Mongo.Repositories.Interfaces;
using Cineverse.Mongo.Schemas.Entities;
using MongoDB.Driver;

namespace Cineverse.Mongo.Repositories.Implementations;

public class UserRepository(
    IMongoDatabase mongoDatabase
) : GenericRepository<UserEntity>(mongoDatabase), IUserRepository
{
    public async Task<UserEntity?> GetUserByCredentialsAsync(string email, string password)
    {
        var passwordHash = HashHelper.ComputeHash(password);

        return await Collection.Find(
            x => x.Email == email &&
            x.PasswordHash == passwordHash
        ).FirstOrDefaultAsync();
    }

    public async Task CreateUserAsync(UserEntity user, string password)
    {
        var passwordHash = HashHelper.ComputeHash(password);

        user.PasswordHash = passwordHash;
        await Collection.InsertOneAsync(user);
    }
}