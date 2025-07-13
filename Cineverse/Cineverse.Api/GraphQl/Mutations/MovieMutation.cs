using Cineverse.API.GraphQl.Base;
using Cineverse.Application.MediatR.Movies.Commands;
using HotChocolate;
using HotChocolate.Types;
using MediatR;

namespace Cineverse.API.GraphQl.Mutations;

[ExtendObjectType(nameof(BaseGraphQlMutation))]
public class MovieMutation
{
    public async Task<bool> CreateMovie(
        [Service] IMediator mediator,
        CreateMovieRequest request
    )
    {
        return await mediator.Send(request);
    }
}