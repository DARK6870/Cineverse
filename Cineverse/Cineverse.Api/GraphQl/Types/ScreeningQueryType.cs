using Cineverse.API.GraphQl.Queries;
using Cineverse.Infrastructure.Authentication;
using HotChocolate.Types;

namespace Cineverse.API.GraphQl.Types;

public class ScreeningQueryType : ObjectTypeExtension<ScreeningQuery>
{
    protected override void Configure(IObjectTypeDescriptor<ScreeningQuery> descriptor)
    {
        if (!AuthenticationSetup.EnableSecurity)
            return;
    }
}