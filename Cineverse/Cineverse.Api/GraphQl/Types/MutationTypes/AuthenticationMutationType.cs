using Cineverse.Api.GraphQl.Mutations;
using Cineverse.Identity;
using Cineverse.Identity.Authentication;
using HotChocolate.Types;

namespace Cineverse.Api.GraphQl.Types.MutationTypes;

public class AuthenticationMutationType : ObjectTypeExtension<AuthenticationMutation>
{
    protected override void Configure(IObjectTypeDescriptor<AuthenticationMutation> descriptor)
    {
        if (!AuthenticationSetup.EnableSecurity)
            return;

        descriptor.Field(x => x.ConfirmEmail(null!, default, CancellationToken.None))
            .Authorize();
        
        descriptor.Field(x => x.DeleteRefreshToken(null!, CancellationToken.None))
            .Authorize();
        
        descriptor.Field(x => x.GenerateEmailVerificationCode(null!, CancellationToken.None))
            .Authorize();
        
        descriptor.Field(x => x.Register(null!, null!, CancellationToken.None))
            .AllowAnonymous();
        
        descriptor.Field(x => x.Login(null!, null!, CancellationToken.None))
            .AllowAnonymous();
        
        descriptor.Field(x => x.GenerateAccessToken(null!, null!, CancellationToken.None))
            .AllowAnonymous();
    }
}