using Cineverse.Mongo.Common.Helpers;
using Cineverse.Mongo.Repositories.Generic;
using Cineverse.Mongo.Schemas.Entities;
using Cineverse.Mongo.Schemas.Enums;
using MongoDB.Driver;
using static MongoDB.Driver.Builders<Cineverse.Mongo.Schemas.Entities.UserEntity>;

namespace Cineverse.Mongo.Repositories.User;

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

    public async Task<bool> UpdateUserStatusAsync(string userId, UserStatus status)
    {
        var result = await Collection.UpdateOneAsync(
            x => x.Id == userId,
            Update.Set(x => x.Status, status)
        );
        
        return result.ModifiedCount > 0;
    }
}