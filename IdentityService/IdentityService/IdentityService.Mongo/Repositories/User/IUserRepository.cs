using IdentityService.Mongo.Schemas.Entities;
using IdentityService.Mongo.Schemas.Enums;
using Infrastructure.Mongo.Repositories.Interfaces.Generic;

namespace IdentityService.Mongo.Repositories.User;

public interface IUserRepository : IGenericRepository<UserEntity>
{
    Task<UserEntity?> GetUserByCredentialsAsync(string email, string password);

    Task CreateUserAsync(UserEntity user, string password);
    
    Task<bool> UpdateUserStatusAsync(string userId, UserStatus status);

    Task<bool> UpdateUserPasswordAsync(string userId, string password);

    Task<bool> UpdateUserPersonalInformationAsync(string userId, string firstName, string lastName);
}