using Cineverse.Api.GraphQl.Base;
using Cineverse.Api.GraphQl.Types;
using Cineverse.Api.GraphQl.Types.MutationTypes;
using Cineverse.Api.GraphQl.Types.QueryTypes;
using HotChocolate.Execution.Configuration;

namespace Cineverse.Api;

public static class Configuration
{
    public static IRequestExecutorBuilder AddGraphQlQueries(this IRequestExecutorBuilder requestExecutorBuilder)
    {
        requestExecutorBuilder
            .AddQueryType<BaseGraphQlQuery>()
            .AddTypeExtension<MovieQueryType>()
            .AddTypeExtension<ScreeningQueryType>()
            .AddTypeExtension<HallQueryType>()
            .AddTypeExtension<BookingQueryType>()
            ;

        return requestExecutorBuilder;
    }

    public static IRequestExecutorBuilder AddGraphQlMutations(this IRequestExecutorBuilder requestExecutorBuilder)
    {
        requestExecutorBuilder
            .AddMutationType<BaseGraphQlMutation>()
            .AddTypeExtension<AuthenticationMutationType>()
            .AddTypeExtension<MovieMutationType>()
            .AddTypeExtension<ScreeningMutationType>()
            .AddTypeExtension<HallMutationType>()
            .AddTypeExtension<BookingMutationType>()
            .AddTypeExtension<SupportMutationType>()
            ;
        
        return requestExecutorBuilder;
    }
}