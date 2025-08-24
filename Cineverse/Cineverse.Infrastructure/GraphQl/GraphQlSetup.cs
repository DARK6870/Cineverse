using Cineverse.Infrastructure.Authentication;
using Cineverse.Infrastructure.Common.Constants;
using HotChocolate.AspNetCore;
using HotChocolate.Execution.Configuration;
using HotChocolate.Types.Descriptors;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Cineverse.Infrastructure.GraphQl;

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
}