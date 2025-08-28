using Cineverse.Api.GraphQl.Mutations;
using Cineverse.Infrastructure.Authentication;
using HotChocolate.Types;

namespace Cineverse.Api.GraphQl.Types;

public class MovieMutationType : ObjectTypeExtension<MovieMutation>
{
    protected override void Configure(IObjectTypeDescriptor<MovieMutation> descriptor)
    {
        if (!AuthenticationSetup.EnableSecurity)
            return;

        descriptor.Authorize(AuthenticationSetup.ManagerAccessPolicy);
    }
}