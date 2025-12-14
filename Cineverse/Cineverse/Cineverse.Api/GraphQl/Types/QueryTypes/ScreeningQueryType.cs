using Auth.Authentication;
using Cineverse.Api.GraphQl.Queries;
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