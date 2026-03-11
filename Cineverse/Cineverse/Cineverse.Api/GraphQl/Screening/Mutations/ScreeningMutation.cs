using Cineverse.Application.MediatR.Requests.Screenings.CreateScreening;
using Cineverse.Application.MediatR.Requests.Screenings.DeleteScreening;
using Cineverse.Application.MediatR.Requests.Screenings.UpdateScreening;
using HotChocolate;
using HotChocolate.Types;
using Infrastructure.WebApi.GraphQl.Base;
using MediatR;

namespace Cineverse.Api.GraphQl.Screening.Mutations;

[ExtendObjectType(nameof(BaseGraphQlMutation))]
public class ScreeningMutation
{
    public async Task<string> CreateScreening(
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