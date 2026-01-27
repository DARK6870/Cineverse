using Cineverse.Application.MediatR.Requests.Contact.CreateContact;
using HotChocolate;
using HotChocolate.Types;
using Infrastructure.WebApi.GraphQl.Base;
using MediatR;

namespace Cineverse.Api.GraphQl.Support.Mutations;

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