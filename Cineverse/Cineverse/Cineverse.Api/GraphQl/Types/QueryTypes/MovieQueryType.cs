using Auth.Authentication;
using Cineverse.Api.GraphQl.Queries;
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