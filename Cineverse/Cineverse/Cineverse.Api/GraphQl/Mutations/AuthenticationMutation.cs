using Cineverse.Application.MediatR.Requests.Authentication.ChangePassword;
using Cineverse.Application.MediatR.Requests.Authentication.ConfirmEmail;
using Cineverse.Application.MediatR.Requests.Authentication.DeleteRefreshToken;
using Cineverse.Application.MediatR.Requests.Authentication.GenerateAccessToken;
using Cineverse.Application.MediatR.Requests.Authentication.GenerateEmailVerificationCode;
using Cineverse.Application.MediatR.Requests.Authentication.Login;
using Cineverse.Application.MediatR.Requests.Authentication.Register;
using Cineverse.Application.MediatR.Requests.Authentication.RestorePassword;
using Cineverse.Infrastructure.Common.Models;
using HotChocolate;
using HotChocolate.Types;
using Infrastructure.WebApi.GraphQl.Base;
using MediatR;

namespace Cineverse.Api.GraphQl.Mutations;
// TODO: refactor all return true

[ExtendObjectType(nameof(BaseGraphQlMutation))]
public class AuthenticationMutation
{
    public async Task<AuthenticationResponse> Register(
        [Service] IMediator mediator,
        RegisterRequest registerRequest,
        CancellationToken cancellationToken
    )
    {
        return await mediator.Send(registerRequest, cancellationToken);
    }

    public async Task<bool> ConfirmEmail(
        [Service] IMediator mediator,
        int verificationCode,
        CancellationToken cancellationToken
    )
    {
        return await mediator.Send(new ConfirmEmailRequest(verificationCode), cancellationToken);
    }

    public async Task<bool> GenerateEmailVerificationCode(
        [Service] IMediator mediator,
        CancellationToken cancellationToken
    )
    {
        return await mediator.Send(new GenerateVerificationCodeRequest(), cancellationToken);
    }
    
    public async Task<AuthenticationResponse> Login(
        [Service] IMediator mediator,
        LoginRequest request,
        CancellationToken cancellationToken
    )
    {
        return await mediator.Send(request, cancellationToken);
    }

    public async Task<AuthenticationResponse> GenerateAccessToken(
        [Service] IMediator mediator,
        string refreshToken,
        CancellationToken cancellationToken
    )
    {
        return await mediator.Send(new GenerateAccessTokenRequest(refreshToken), cancellationToken);
    }
    
    public async Task<bool> DeleteRefreshToken(
        [Service] IMediator mediator,
        CancellationToken cancellationToken
    )
    {
        return await mediator.Send(new DeleteRefreshTokenRequest(), cancellationToken);
    }

    public async Task<bool> ChangePassword(
        [Service] IMediator mediator,
        ChangePasswordRequest request,
        CancellationToken cancellationToken
    )
    {
        return await mediator.Send(request, cancellationToken);
    }

    public async Task<bool> SendRestorePasswordEmail(
        [Service] IMediator mediator,
        string email,
        CancellationToken cancellationToken
    )
    {
        return await mediator.Send(new SendRestorePasswordEmailRequest(email), cancellationToken);
    }

    public async Task<bool> RestorePassword(
        [Service] IMediator mediator,
        RestorePasswordRequest request,
        CancellationToken cancellationToken
    )
    {
        return await mediator.Send(request, cancellationToken);
    }
}