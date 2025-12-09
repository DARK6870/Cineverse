using Cineverse.Api.GraphQl.Queries;
using Cineverse.Identity.Authentication;
using HotChocolate.Types;

namespace Cineverse.Api.GraphQl.Types.QueryTypes;

public class ScreeningQueryType : ObjectTypeExtension<ScreeningQuery>
{
    protected override void Configure(IObjectTypeDescriptor<ScreeningQuery> descriptor)
    {
        if (!AuthenticationSetup.EnableSecurity)
            return;
    }
}