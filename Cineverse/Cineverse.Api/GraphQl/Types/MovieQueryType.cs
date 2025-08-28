using Cineverse.Api.GraphQl.Queries;
using Cineverse.Infrastructure.Authentication;
using HotChocolate.Types;

namespace Cineverse.Api.GraphQl.Types;

public class MovieQueryType : ObjectTypeExtension<MovieQuery>
{
    protected override void Configure(IObjectTypeDescriptor<MovieQuery> descriptor)
    {
        if (!AuthenticationSetup.EnableSecurity)
            return;
    }
}