using Auth.Authentication;
using Cineverse.Api.GraphQl.Mutations;
using HotChocolate.Types;

namespace Cineverse.Api.GraphQl.Types.MutationTypes;

public class HallMutationType : ObjectTypeExtension<HallMutation>
{
    protected override void Configure(IObjectTypeDescriptor<HallMutation> descriptor)
    {
        if (!AuthenticationSetup.EnableSecurity)
            return;

        descriptor.Field(x => x.CreateHall(null!, null!, CancellationToken.None))
            .Authorize(AuthenticationPolicies.ManagerAccessPolicy);
        
        descriptor.Field(x => x.UpdateHall(null!, null!, CancellationToken.None))
            .Authorize(AuthenticationPolicies.ManagerAccessPolicy);
        
        descriptor.Field(x => x.DeleteHall(null!, null!, CancellationToken.None))
            .Authorize(AuthenticationPolicies.ManagerAccessPolicy);
    }
}