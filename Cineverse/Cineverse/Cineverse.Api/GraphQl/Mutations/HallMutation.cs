using Cineverse.Application.MediatR.Requests.Hall.CreateHall;
using Cineverse.Application.MediatR.Requests.Hall.DeleteHall;
using Cineverse.Application.MediatR.Requests.Hall.UpdateHall;
using HotChocolate;
using HotChocolate.Types;
using Infrastructure.WebApi.GraphQl.Base;
using MediatR;

namespace Cineverse.Api.GraphQl.Mutations;

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