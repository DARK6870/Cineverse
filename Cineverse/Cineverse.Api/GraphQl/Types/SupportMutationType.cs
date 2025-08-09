using Cineverse.API.GraphQl.Mutations;
using Cineverse.Infrastructure.Authentication;
using HotChocolate.Types;

namespace Cineverse.API.GraphQl.Types;

public class SupportMutationType : ObjectTypeExtension<SupportMutation>
{
    protected override void Configure(IObjectTypeDescriptor<SupportMutation> descriptor)
    {
        if (!AuthenticationSetup.EnableSecurity)
            return;
    }
}