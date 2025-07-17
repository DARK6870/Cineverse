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
        CreateHallRequest createHallRequest
    )
    {
        return await mediator.Send(createHallRequest);
    }
    
    public async Task<bool> UpdateHall(
        [Service] IMediator mediator,
        UpdateHallRequest updateHallRequest
    )
    {
        return await mediator.Send(updateHallRequest);
    }
    
    public async Task<bool> DeleteHall(
        [Service] IMediator mediator,
        string id
    )
    {
        return await mediator.Send(new DeleteHallRequest(id));
    }
}