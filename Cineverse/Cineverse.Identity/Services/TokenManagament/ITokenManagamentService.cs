using Cineverse.Mongo.Schemas.Entities;

namespace Cineverse.Identity.Services.TokenManagament;

public interface ITokenManagamentService
{
    string GenerateJwtToken(UserEntity user);
}