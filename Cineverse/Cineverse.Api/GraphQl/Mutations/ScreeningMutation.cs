using Cineverse.API.GraphQl.Base;
using Cineverse.Application.MediatR.Screenings.Commands;
using HotChocolate;
using HotChocolate.Types;
using MediatR;

namespace Cineverse.API.GraphQl.Mutations;

[ExtendObjectType(nameof(BaseGraphQlMutation))]
public class ScreeningMutation
{
    public async Task<bool> CreateScreening(
        [Service] IMediator mediator,
        CreateScreeningRequest request,
        CancellationToken cancellationToken
    )
    {
        return await mediator.Send(request, cancellationToken);
    }
    
    public async Task<bool> UpdateScreening(
        [Service] IMediator mediator,
        UpdateScreeningRequest request,
        CancellationToken cancellationToken
    )
    {
        return await mediator.Send(request, cancellationToken);
    }
    
    public async Task<bool> DeleteScreening(
        [Service] IMediator mediator,
        string id,
        CancellationToken cancellationToken
    )
    {
        return await mediator.Send(new DeleteScreeningRequest(id), cancellationToken);
    }
}