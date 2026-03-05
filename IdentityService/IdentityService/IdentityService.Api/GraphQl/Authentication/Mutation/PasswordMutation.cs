using HotChocolate;
using HotChocolate.Types;
using IdentityService.Application.MediatR.Requests.Authentication.ChangePassword;
using IdentityService.Application.MediatR.Requests.Authentication.RestorePassword;
using Infrastructure.WebApi.GraphQl.Base;
using MediatR;

namespace IdentityService.Api.GraphQl.Authentication.Mutation;

[ExtendObjectType(nameof(BaseGraphQlMutation))]
public class PasswordMutation
{
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