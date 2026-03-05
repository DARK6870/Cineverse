using Auth.Authentication;
using HotChocolate.Types;
using IdentityService.Api.GraphQl.Authentication.Mutation;

namespace IdentityService.Api.GraphQl.Authentication.Types;

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