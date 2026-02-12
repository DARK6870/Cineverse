using Auth.Authentication;
using Cineverse.Api.GraphQl.Hall.Queries;
using HotChocolate.Types;

namespace Cineverse.Api.GraphQl.Hall.Types;

public class HallQueryType : ObjectTypeExtension<HallQuery>
{
    protected override void Configure(IObjectTypeDescriptor<HallQuery> descriptor)
    {
        if (!AuthenticationSetup.EnableSecurity)
            return;
    }
}