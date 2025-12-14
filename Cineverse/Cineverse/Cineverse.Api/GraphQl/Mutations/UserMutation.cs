using Cineverse.Application.MediatR.Requests.Users.UpdatePersonalInformation;
using HotChocolate;
using HotChocolate.Types;
using Infrastructure.WebApi.GraphQl.Base;
using MediatR;

namespace Cineverse.Api.GraphQl.Mutations;

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