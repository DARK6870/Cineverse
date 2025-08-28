using Cineverse.Api.GraphQl.Queries;
using Cineverse.Infrastructure.Authentication;
using HotChocolate.Types;

namespace Cineverse.Api.GraphQl.Types;

public class ScreeningQueryType : ObjectTypeExtension<ScreeningQuery>
{
    protected override void Configure(IObjectTypeDescriptor<ScreeningQuery> descriptor)
    {
        if (!AuthenticationSetup.EnableSecurity)
            return;
    }
}