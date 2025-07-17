using Cineverse.API.GraphQl.Base;
using Cineverse.API.GraphQl.Types;
using HotChocolate.Execution.Configuration;

namespace Cineverse.API;

public static class Configuration
{
    public static IRequestExecutorBuilder AddGraphQlQueries(this IRequestExecutorBuilder requestExecutorBuilder)
    {
        requestExecutorBuilder
            .AddQueryType<BaseGraphQlQuery>()
            .AddTypeExtension<MovieQueryType>()
            .AddTypeExtension<ScreeningQueryType>()
            .AddTypeExtension<HallQueryType>()
            ;

        return requestExecutorBuilder;
    }

    public static IRequestExecutorBuilder AddGraphQlMutations(this IRequestExecutorBuilder requestExecutorBuilder)
    {
        requestExecutorBuilder
            .AddMutationType<BaseGraphQlMutation>()
            .AddTypeExtension<MovieMutationType>()
            .AddTypeExtension<AuthenticationMutationType>()
            .AddTypeExtension<ScreeningMutationType>()
            .AddTypeExtension<HallMutationType>()
            ;
        
        return requestExecutorBuilder;
    }
}