using Cineverse.API.GraphQl.Base;
using Cineverse.Application.MediatR.Hall.Commands;
using HotChocolate;
using HotChocolate.Types;
using MediatR;

namespace Cineverse.API.GraphQl.Mutations;

[ExtendObjectType(nameof(BaseGraphQlMutation))]
public class HallMutation
{
    public async Task<bool> CreateHall(
        [Service] IMediator mediator,
        CreateHallRequest request,
        CancellationToken cancellationToken
    )
    {
        return await mediator.Send(request, cancellationToken);
    }
    
    public async Task<bool> UpdateHall(
        [Service] IMediator mediator,
        UpdateHallRequest request,
        CancellationToken cancellationToken
    )
    {
        return await mediator.Send(request, cancellationToken);
    }
    
    public async Task<bool> DeleteHall(
        [Service] IMediator mediator,
        string id,
        CancellationToken cancellationToken
    )
    {
        return await mediator.Send(new DeleteHallRequest(id), cancellationToken);
    }
}