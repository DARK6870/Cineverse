using Cineverse.API.GraphQl.Mutations;
using Cineverse.Infrastructure.Authentication;
using HotChocolate.Types;

namespace Cineverse.API.GraphQl.Types;

public class ScreeningMutationType  : ObjectTypeExtension<ScreeningMutation>
{
    protected override void Configure(IObjectTypeDescriptor<ScreeningMutation> descriptor)
    {
        if (!AuthenticationSetup.EnableSecurity)
            return;

        descriptor.Authorize(AuthenticationSetup.ManagerAccessPolicy);
    }
}