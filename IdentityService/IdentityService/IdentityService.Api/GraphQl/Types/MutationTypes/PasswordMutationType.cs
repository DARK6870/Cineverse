using Auth.Authentication;
using HotChocolate.Types;
using IdentityService.Api.GraphQl.Mutations;

namespace IdentityService.Api.GraphQl.Types.MutationTypes;

public class PasswordMutationType : ObjectTypeExtension<PasswordMutation>
{
    protected override void Configure(IObjectTypeDescriptor<PasswordMutation> descriptor)
    {
        if (!AuthenticationSetup.EnableSecurity)
            return;

        descriptor.Field(x => x.ChangePassword(null!, null!, CancellationToken.None))
            .Authorize();
        
        descriptor.Field(x => x.SendRestorePasswordEmail(null!, null!, CancellationToken.None))
            .AllowAnonymous();
        
        descriptor.Field(x => x.RestorePassword(null!, null!, CancellationToken.None))
            .AllowAnonymous();
    }
}