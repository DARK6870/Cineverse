using Cineverse.API.GraphQl.Base;
using Cineverse.Application.MediatR.Authentication.Commands;
using Cineverse.Infrastructure.Common.Models;
using Cineverse.Notifications.Common.Builders;
using Cineverse.Notifications.Services.Interfaces;
using HotChocolate;
using HotChocolate.Types;
using MediatR;

namespace Cineverse.API.GraphQl.Mutations;

[ExtendObjectType(nameof(BaseGraphQlMutation))]
public class AuthenticationMutation
{
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
        await service.SendEmailNotification("tsymbalvlad.6870@gmail.com", "test", new MessageBuilder { Message = "test" });
        return await mediator.Send(new LogoutRequest(), cancellationToken);
    }
}