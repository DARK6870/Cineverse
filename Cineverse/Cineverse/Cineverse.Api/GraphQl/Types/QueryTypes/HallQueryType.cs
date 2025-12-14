using Auth.Authentication;
using Cineverse.Api.GraphQl.Queries;
using HotChocolate.Types;

namespace Cineverse.Api.GraphQl.Types.QueryTypes;

public class HallQueryType : ObjectTypeExtension<HallQuery>
{
    protected override void Configure(IObjectTypeDescriptor<HallQuery> descriptor)
    {
        if (!AuthenticationSetup.EnableSecurity)
            return;
    }
}