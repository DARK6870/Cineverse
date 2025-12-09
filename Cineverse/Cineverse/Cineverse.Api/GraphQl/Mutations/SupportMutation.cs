using Cineverse.Api.GraphQl.Base;
using Cineverse.Application.MediatR.Requests.Contact.CreateContact;
using HotChocolate;
using HotChocolate.Types;
using MediatR;

namespace Cineverse.Api.GraphQl.Mutations;

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