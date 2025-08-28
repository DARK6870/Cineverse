using Cineverse.Api.GraphQl.Queries;
using Cineverse.Infrastructure.Authentication;
using HotChocolate.Types;

namespace Cineverse.Api.GraphQl.Types;

public class HallQueryType : ObjectTypeExtension<HallQuery>
{
    protected override void Configure(IObjectTypeDescriptor<HallQuery> descriptor)
    {
        if (!AuthenticationSetup.EnableSecurity)
            return;
    }
}