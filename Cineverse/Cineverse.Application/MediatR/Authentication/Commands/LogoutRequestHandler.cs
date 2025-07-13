using System.Net;
using Cineverse.Infrastructure.Common.Constants;
using Cineverse.Infrastructure.Common.Exceptions;
using Cineverse.Infrastructure.Services.Interfaces;
using Cineverse.Mongo.Repositories.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Cineverse.Application.MediatR.Authentication.Commands;

public record LogoutRequest : IRequest<bool>;

public class LogoutRequestHandler(
    IRefreshTokenRepository refreshTokenRepository,
    IUserContext userContext
)
{
    public async Task<bool> Handle(LogoutRequest request, CancellationToken cancellationToken)
    {
        var cookieRefreshToken = userContext.GetRefreshTokenFromCookie();
        
        await refreshTokenRepository.DeleteOneAsync(x => x.Token == cookieRefreshToken, cancellationToken);
        userContext.RemoveRefreshTokenFromCookie();

        return true;
    }
}