using Cineverse.Api.GraphQl.Mutations;
using Cineverse.Identity;
using Cineverse.Identity.Authentication;
using HotChocolate.Types;

namespace Cineverse.Api.GraphQl.Types.MutationTypes;

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