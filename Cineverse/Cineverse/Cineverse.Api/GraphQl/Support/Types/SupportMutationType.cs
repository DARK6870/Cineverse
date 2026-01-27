using Auth.Authentication;
using Cineverse.Api.GraphQl.Support.Mutations;
using HotChocolate.Types;

namespace Cineverse.Api.GraphQl.Support.Types;

public class SupportMutationType : ObjectTypeExtension<SupportMutation>
{
    protected override void Configure(IObjectTypeDescriptor<SupportMutation> descriptor)
    {
        if (!AuthenticationSetup.EnableSecurity)
            return;
    }
}