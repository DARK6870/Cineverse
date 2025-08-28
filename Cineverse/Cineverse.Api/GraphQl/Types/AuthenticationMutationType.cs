using Cineverse.Api.GraphQl.Mutations;
using Cineverse.Infrastructure.Authentication;
using HotChocolate.Types;

namespace Cineverse.Api.GraphQl.Types;

public class AuthenticationMutationType : ObjectTypeExtension<AuthenticationMutation>
{
    protected override void Configure(IObjectTypeDescriptor<AuthenticationMutation> descriptor)
    {
        if (!AuthenticationSetup.EnableSecurity)
            return;

        descriptor.Field(x => x.Register(null!, null!, CancellationToken.None))
            .AllowAnonymous();
        
        descriptor.Field(x => x.Login(null!, null!, CancellationToken.None))
            .AllowAnonymous();
        
        descriptor.Field(x => x.GenerateAccessToken(null!, null!, CancellationToken.None))
            .AllowAnonymous();
    }
}