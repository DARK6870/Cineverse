using Cineverse.API.GraphQl.Queries;
using Cineverse.Infrastructure.Authentication;
using HotChocolate.Types;

namespace Cineverse.API.GraphQl.Types;

public class HallQueryType : ObjectTypeExtension<HallQuery>
{
    protected override void Configure(IObjectTypeDescriptor<HallQuery> descriptor)
    {
        if (!AuthenticationSetup.EnableSecurity)
            return;
    }
}