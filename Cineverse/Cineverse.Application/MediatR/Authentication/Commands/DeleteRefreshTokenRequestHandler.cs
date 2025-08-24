using Cineverse.Infrastructure.Services.Interfaces;
using Cineverse.Mongo.Repositories.Interfaces;
using MediatR;

namespace Cineverse.Application.MediatR.Authentication.Commands;

public record DeleteRefreshTokenRequest : IRequest<bool>;

public class DeleteRefreshTokenRequestHandler(
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