using Cineverse.Api.GraphQl.Mutations;
using Cineverse.Infrastructure.Authentication;
using HotChocolate.Types;

namespace Cineverse.Api.GraphQl.Types;

public class SupportMutationType : ObjectTypeExtension<SupportMutation>
{
    protected override void Configure(IObjectTypeDescriptor<SupportMutation> descriptor)
    {
        if (!AuthenticationSetup.EnableSecurity)
            return;
    }
}