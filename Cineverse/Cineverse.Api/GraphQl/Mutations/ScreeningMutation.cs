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
        CreateScreeningRequest createScreeningRequest
    )
    {
        return await mediator.Send(createScreeningRequest);
    }
    
    public async Task<bool> UpdateScreening(
        [Service] IMediator mediator,
        UpdateScreeningRequest updateScreeningRequest
    )
    {
        return await mediator.Send(updateScreeningRequest);
    }
    
    public async Task<bool> DeleteScreening(
        [Service] IMediator mediator,
        string id
    )
    {
        return await mediator.Send(new DeleteScreeningRequest(id));
    }
}