using HotChocolate;
using HotChocolate.Types;
using IdentityService.Application.MediatR.Requests.Users.UpdatePersonalInformation;
using Infrastructure.WebApi.GraphQl.Base;
using MediatR;

namespace IdentityService.Api.GraphQl.Mutations;

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
}