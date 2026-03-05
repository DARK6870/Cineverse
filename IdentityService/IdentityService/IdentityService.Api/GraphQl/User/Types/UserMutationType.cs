using Auth.Authentication;
using HotChocolate.Types;
using IdentityService.Api.GraphQl.User.Mutation;

namespace IdentityService.Api.GraphQl.User.Types;

public class UserMutationType : ObjectTypeExtension<UserMutation>
{
    protected override void Configure(IObjectTypeDescriptor<UserMutation> descriptor)
    {
        if (!AuthenticationSetup.EnableSecurity)
            return;

        descriptor
            .Field(x => x.UpdatePersonalInformation(null!, null!, CancellationToken.None))
            .Authorize();
        
        descriptor
            .Field(x => x.UpdateUser(null!, null!, CancellationToken.None))
            .Authorize(AuthenticationPolicies.ManagerAccessPolicy);
    }
}