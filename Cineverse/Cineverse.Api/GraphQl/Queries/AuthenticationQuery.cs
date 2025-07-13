using Cineverse.API.GraphQl.Base;
using Cineverse.Application.Common.Models;
using Cineverse.Application.MediatR.Authentication.Commands;
using HotChocolate;
using HotChocolate.Types;
using MediatR;

namespace Cineverse.API.GraphQl.Queries;

[ExtendObjectType(nameof(BaseGraphQlQuery))]
public class AuthenticationQuery
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
        CancellationToken cancellationToken
    )
    {
        return await mediator.Send(new LogoutRequest(), cancellationToken);
    }
}