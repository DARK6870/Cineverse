using Cineverse.Api.GraphQl.Mutations;
using Cineverse.Identity.Authentication;
using HotChocolate.Types;

namespace Cineverse.Api.GraphQl.Types.MutationTypes;

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