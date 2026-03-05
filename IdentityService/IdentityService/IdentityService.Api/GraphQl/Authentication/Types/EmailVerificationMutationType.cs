using Auth.Authentication;
using HotChocolate.Types;
using IdentityService.Api.GraphQl.Authentication.Mutation;

namespace IdentityService.Api.GraphQl.Authentication.Types;

public class EmailVerificationMutationType : ObjectTypeExtension<EmailVerificationMutation>
{
    protected override void Configure(IObjectTypeDescriptor<EmailVerificationMutation> descriptor)
    {
        if (!AuthenticationSetup.EnableSecurity)
            return;

        descriptor.Field(x => x.ConfirmEmail(null!, default, CancellationToken.None))
            .Authorize();
        
        descriptor.Field(x => x.GenerateEmailVerificationCode(null!, CancellationToken.None))
            .Authorize();
    }
}