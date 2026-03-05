using HotChocolate;
using HotChocolate.Types;
using IdentityService.Application.MediatR.Requests.Users.UpdatePersonalInformation;
using IdentityService.Application.MediatR.Requests.Users.UpdateUser;
using Infrastructure.WebApi.GraphQl.Base;
using MediatR;

namespace IdentityService.Api.GraphQl.User.Mutation;

[ExtendObjectType(nameof(BaseGraphQlMutation))]
public class UserMutation
{
    public async Task<bool> UpdatePersonalInformation(
        [Service] IMediator mediator,
        UpdatePersonalInformationRequest request,
        CancellationToken cancellationToken
    )
    {
        return await mediator.Send(request, cancellationToken);
    }

    public async Task<bool> UpdateUser(
        UpdateUserRequest request,
        [Service] IMediator mediator,
        CancellationToken cancellationToken
    )
    {
        return await mediator.Send(request, cancellationToken);
    }
}