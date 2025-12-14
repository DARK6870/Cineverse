using Auth.Authentication;
using Cineverse.Api.GraphQl.Mutations;
using HotChocolate.Types;

namespace Cineverse.Api.GraphQl.Types.MutationTypes;

public class ScreeningMutationType  : ObjectTypeExtension<ScreeningMutation>
{
    protected override void Configure(IObjectTypeDescriptor<ScreeningMutation> descriptor)
    {
        if (!AuthenticationSetup.EnableSecurity)
            return;

        descriptor.Field(x => x.CreateScreening(null!, null!, CancellationToken.None))
            .Authorize(AuthenticationPolicies.ManagerAccessPolicy);
        
        descriptor.Field(x => x.UpdateScreening(null!, null!, CancellationToken.None))
            .Authorize(AuthenticationPolicies.ManagerAccessPolicy);
        
        descriptor.Field(x => x.DeleteScreening(null!, null!, CancellationToken.None))
            .Authorize(AuthenticationPolicies.ManagerAccessPolicy);
    }
}