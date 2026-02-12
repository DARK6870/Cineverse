using Auth.Authentication;
using Cineverse.Api.GraphQl.Movie.Mutations;
using HotChocolate.Types;

namespace Cineverse.Api.GraphQl.Movie.Types;

public class MovieMutationType : ObjectTypeExtension<MovieMutation>
{
    protected override void Configure(IObjectTypeDescriptor<MovieMutation> descriptor)
    {
        if (!AuthenticationSetup.EnableSecurity)
            return;

        descriptor.Field(x => x.CreateMovie(null!, null!, CancellationToken.None))
            .Authorize(AuthenticationPolicies.ManagerAccessPolicy);
        
        descriptor.Field(x => x.UpdateMovie(null!, null!, CancellationToken.None))
            .Authorize(AuthenticationPolicies.ManagerAccessPolicy);
        
        descriptor.Field(x => x.DeleteMovie(null!, null!, CancellationToken.None))
            .Authorize(AuthenticationPolicies.ManagerAccessPolicy);
    }
}