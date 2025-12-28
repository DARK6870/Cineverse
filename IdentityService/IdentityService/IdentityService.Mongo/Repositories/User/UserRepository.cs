using Auth.Models.Enums;
using IdentityService.Mongo.Schemas.Entities;
using Infrastructure.Common.Helpers;
using Infrastructure.Mongo.Repositories.Implementations;
using MongoDB.Driver;
using static MongoDB.Driver.Builders<IdentityService.Mongo.Schemas.Entities.UserEntity>;

namespace IdentityService.Mongo.Repositories.User;

public class UserRepository(
    IMongoDatabase mongoDatabase
) : GenericRepository<UserEntity>(mongoDatabase), IUserRepository
{
    public async Task<UserEntity?> GetUserByCredentialsAsync(string email, string password)
    {
        var passwordHash = HashHelper.ComputeSha256(password);

        return await Collection.Find(
            x => x.Email == email &&
            x.PasswordHash == passwordHash
        ).FirstOrDefaultAsync();
    }

    public async Task CreateUserAsync(UserEntity user, string password)
    {
        var passwordHash = HashHelper.ComputeSha256(password);

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

    public async Task<bool> UpdateUserPasswordAsync(string userId, string password)
    {
        var passwordHash = HashHelper.ComputeSha256(password);
        
        var result = await Collection.UpdateOneAsync(
            x => x.Id == userId,
            Update.Set(x => x.PasswordHash, passwordHash)
        );
        
        return result.ModifiedCount > 0;
    }

    public async Task<bool> UpdateUserPersonalInformationAsync(string userId, string firstName, string lastName)
    {
        var result = await Collection.UpdateOneAsync(
            x => x.Id == userId,
            Update
                .Set(x => x.FirstName, firstName)
                .Set(x => x.LastName, lastName)
        );
        
        return result.ModifiedCount > 0;
    }
}