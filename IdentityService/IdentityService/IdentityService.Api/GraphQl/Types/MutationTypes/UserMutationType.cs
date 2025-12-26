using Auth.Authentication;
using HotChocolate.Types;
using IdentityService.Api.GraphQl.Mutations;

namespace IdentityService.Api.GraphQl.Types.MutationTypes;

public class UserMutationType : ObjectTypeExtension<UserMutation>
{
    protected override void Configure(IObjectTypeDescriptor<UserMutation> descriptor)
    {
        if (!AuthenticationSetup.EnableSecurity)
            return;

        descriptor.Field(x => x.UpdatePersonalInformation(null!, null!, CancellationToken.None))
            .Authorize();
    }
}