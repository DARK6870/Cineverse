using Cineverse.API.GraphQl.Queries;
using Cineverse.Infrastructure.Authentication;
using HotChocolate.Types;

namespace Cineverse.API.GraphQl.Types;

public class MovieQueryType : ObjectTypeExtension<MovieQuery>
{
    protected override void Configure(IObjectTypeDescriptor<MovieQuery> descriptor)
    {
        if (!AuthenticationSetup.EnableSecurity)
            return;
    }
}