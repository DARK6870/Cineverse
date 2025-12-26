using IdentityService.Mongo.Repositories.RefreshToken;
using Infrastructure.Context.UserContext;
using MediatR;

namespace IdentityService.Application.MediatR.Requests.Authentication.DeleteRefreshToken;

public class DeleteRefreshTokenHandler(
    IUserContext userContext,
    IRefreshTokenRepository refreshTokenRepository
) : IRequestHandler<DeleteRefreshTokenRequest, bool>
{
    public async Task<bool> Handle(DeleteRefreshTokenRequest request, CancellationToken cancellationToken)
    {
        await refreshTokenRepository
            .DeleteOneAsync(
                x => x.UserId == userContext.UserId &&
                     x.IpAddress == userContext.IpAddress,
                cancellationToken
            );
        
        return true;
    }
}