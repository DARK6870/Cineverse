using Auth.Authentication;
using Cineverse.Api.GraphQl.Screening.Queries;
using HotChocolate.Types;

namespace Cineverse.Api.GraphQl.Screening.Types;

public class ScreeningQueryType : ObjectTypeExtension<ScreeningQuery>
{
    protected override void Configure(IObjectTypeDescriptor<ScreeningQuery> descriptor)
    {
        if (!AuthenticationSetup.EnableSecurity)
            return;
    }
}