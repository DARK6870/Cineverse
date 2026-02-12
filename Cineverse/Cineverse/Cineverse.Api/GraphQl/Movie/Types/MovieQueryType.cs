using Auth.Authentication;
using Cineverse.Api.GraphQl.Movie.Queries;
using HotChocolate.Types;

namespace Cineverse.Api.GraphQl.Movie.Types;

public class MovieQueryType : ObjectTypeExtension<MovieQuery>
{
    protected override void Configure(IObjectTypeDescriptor<MovieQuery> descriptor)
    {
        if (!AuthenticationSetup.EnableSecurity)
            return;
    }
}