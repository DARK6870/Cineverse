using Cineverse.Api.GraphQl.Queries;
using Cineverse.Identity;
using Cineverse.Identity.Authentication;
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