using HotChocolate;
using HotChocolate.Types;
using IdentityService.Application.MediatR.Requests.Authentication.ConfirmEmail;
using IdentityService.Application.MediatR.Requests.Authentication.GenerateEmailVerificationCode;
using Infrastructure.WebApi.GraphQl.Base;
using MediatR;

namespace IdentityService.Api.GraphQl.Mutations;

[ExtendObjectType(nameof(BaseGraphQlMutation))]
public class EmailVerificationMutation
{
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
}