using Cineverse.Api.GraphQl.Base;
using Cineverse.Application.MediatR.Requests.Movies.CreateMovie;
using Cineverse.Application.MediatR.Requests.Movies.DeleteMovie;
using Cineverse.Application.MediatR.Requests.Movies.UpdateMovie;
using HotChocolate;
using HotChocolate.Types;
using MediatR;

namespace Cineverse.Api.GraphQl.Mutations;

[ExtendObjectType(nameof(BaseGraphQlMutation))]
public class MovieMutation
{
    public async Task<bool> CreateMovie(
        [Service] IMediator mediator,
        CreateMovieRequest request,
        CancellationToken cancellationToken
    )
    {
        return await mediator.Send(request, cancellationToken);
    }
    
    public async Task<bool> UpdateMovie(
        [Service] IMediator mediator,
        UpdateMovieRequest request,
        CancellationToken cancellationToken
    )
    {
        return await mediator.Send(request, cancellationToken);
    }
    
    public async Task<bool> DeleteMovie(
        [Service] IMediator mediator,
        string id,
        CancellationToken cancellationToken
    )
    {
        return await mediator.Send(new DeleteMovieRequest(id), cancellationToken);
    }
}