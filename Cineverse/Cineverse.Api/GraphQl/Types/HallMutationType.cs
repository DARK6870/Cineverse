using Cineverse.Api.GraphQl.Mutations;
using Cineverse.Infrastructure.Authentication;
using HotChocolate.Types;

namespace Cineverse.Api.GraphQl.Types;

public class HallMutationType  : ObjectTypeExtension<HallMutation>
{
    protected override void Configure(IObjectTypeDescriptor<HallMutation> descriptor)
    {
        if (!AuthenticationSetup.EnableSecurity)
            return;

        descriptor.Authorize(AuthenticationSetup.ManagerAccessPolicy);
    }
}