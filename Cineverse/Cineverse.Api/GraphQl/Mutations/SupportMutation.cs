using Cineverse.API.GraphQl.Base;
using Cineverse.Application.MediatR.Contact.Commands;
using HotChocolate;
using HotChocolate.Types;
using MediatR;

namespace Cineverse.API.GraphQl.Mutations;

[ExtendObjectType(nameof(BaseGraphQlMutation))]
public class SupportMutation
{
    public async Task<bool> CreateContactRequest(
        [Service] IMediator mediator,
        CreateContactRequest request,
        CancellationToken cancellationToken
    )
    {
        return await mediator.Send(request, cancellationToken);
    }
}