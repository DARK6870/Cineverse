using Cineverse.API.GraphQl.Base;
using Cineverse.Application.MediatR.Authentication.Commands;
using Cineverse.Infrastructure.Common.Models;
using Cineverse.Notifications.Services.Interfaces;
using HotChocolate;
using HotChocolate.Types;
using MediatR;

namespace Cineverse.API.GraphQl.Mutations;

[ExtendObjectType(nameof(BaseGraphQlMutation))]
public class AuthenticationMutation
{
    public async Task<LoginResponse> Register(
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
    
    public async Task<LoginResponse> Login(
        [Service] IMediator mediator,
        LoginRequest request,
        CancellationToken cancellationToken
    )
    {
        return await mediator.Send(request, cancellationToken);
    }
    
    public async Task<bool> Logout(
        [Service] IMediator mediator,
        [Service] INotificationService service,
        CancellationToken cancellationToken
    )
    {
        return await mediator.Send(new LogoutRequest(), cancellationToken);
    }
}