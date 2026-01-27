using Auth.Authentication;
using Cineverse.Api.GraphQl.Screening.Mutations;
using HotChocolate.Types;

namespace Cineverse.Api.GraphQl.Screening.Types;

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