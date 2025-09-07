using Cineverse.Api.GraphQl.Base;
using Cineverse.Api.GraphQl.Types.MutationTypes;
using Cineverse.Api.GraphQl.Types.QueryTypes;
using Cineverse.Identity.Authentication;
using Cineverse.Infrastructure.Common.Constants;
using Cineverse.Infrastructure.GraphQl;
using HotChocolate.AspNetCore;
using HotChocolate.Execution.Configuration;
using HotChocolate.Types.Descriptors;

namespace Cineverse.Api.GraphQl;

public static class GraphQlSetup
{
    public static IRequestExecutorBuilder ConfigureGraphQl(this IRequestExecutorBuilder requestExecutorBuilder)
    {
        if (AuthenticationSetup.EnableSecurity)
            requestExecutorBuilder.AddAuthorization();
        
        requestExecutorBuilder
            .ModifyPagingOptions(options =>
            {
                options.DefaultPageSize = GraphQlConstants.DefaultPageSize;
                options.MaxPageSize = GraphQlConstants.MaxPageSize;
                options.IncludeTotalCount = true;
            })
            .AddFiltering()
            .AddSorting()
            .AddProjections()
            .AddQueryableOffsetPagingProvider(GraphQlConstants.QueryablePaginationProvider)
            .DisableIntrospection(false)
            .AddErrorFilter<GraphQlErrorFilter>()
            .AddConvention<INamingConventions>(new ApplicationNamingConvention())
            ;

        return requestExecutorBuilder;
    }

    public static void MapCineverseGraphQl(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder
            .MapNitroApp(GraphQlConstants.NitroAppPath)
            .WithOptions(new GraphQLToolOptions()
            {
                DisableTelemetry = true,
                GaTrackingId = null
            });

        endpointRouteBuilder.MapGraphQL(GraphQlConstants.GraphQlPath);
    }
    
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