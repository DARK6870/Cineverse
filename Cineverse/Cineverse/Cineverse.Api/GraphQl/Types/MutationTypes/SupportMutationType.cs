using Auth.Authentication;
using Cineverse.Api.GraphQl.Mutations;
using HotChocolate.Types;

namespace Cineverse.Api.GraphQl.Types.MutationTypes;

public class SupportMutationType : ObjectTypeExtension<SupportMutation>
{
    protected override void Configure(IObjectTypeDescriptor<SupportMutation> descriptor)
    {
        if (!AuthenticationSetup.EnableSecurity)
            return;
    }
}