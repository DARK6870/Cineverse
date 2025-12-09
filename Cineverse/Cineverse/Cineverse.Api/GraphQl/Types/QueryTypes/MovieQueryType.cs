using Cineverse.Api.GraphQl.Queries;
using Cineverse.Identity.Authentication;
using HotChocolate.Types;

namespace Cineverse.Api.GraphQl.Types.QueryTypes;

public class MovieQueryType : ObjectTypeExtension<MovieQuery>
{
    protected override void Configure(IObjectTypeDescriptor<MovieQuery> descriptor)
    {
        if (!AuthenticationSetup.EnableSecurity)
            return;
    }
}