using System.Reflection;
using Auth.Authentication;
using HotChocolate.AspNetCore;
using HotChocolate.Execution.Configuration;
using HotChocolate.Types.Descriptors;
using Infrastructure.WebApi.GraphQl.Base;
using Infrastructure.WebApi.GraphQl.Constants;
using Infrastructure.WebApi.GraphQl.ErrorFilters;
using Infrastructure.WebApi.GraphQl.Extensions;
using Infrastructure.WebApi.GraphQl.Middlewares;
using Infrastructure.WebApi.GraphQl.NamingConventions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.WebApi.GraphQl;

public static class GraphQlExtensions
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

    public static IRequestExecutorBuilder AddGraphQlQueries(this IRequestExecutorBuilder requestExecutorBuilder)
    {
        var queryExtensions = Assembly.GetExecutingAssembly()
            .GetGraphQlExtensions<BaseGraphQlQuery>();

        requestExecutorBuilder
            .AddQueryType<BaseGraphQlQuery>()
            .AddTypes(queryExtensions);

        return requestExecutorBuilder;
    }
    
    public static IRequestExecutorBuilder AddGraphQlMutations(this IRequestExecutorBuilder requestExecutorBuilder)
    {
        var mutationExtensions = Assembly.GetExecutingAssembly()
            .GetGraphQlExtensions<BaseGraphQlMutation>();

        requestExecutorBuilder
            .AddMutationType<BaseGraphQlMutation>()
            .AddTypes(mutationExtensions);

        return requestExecutorBuilder;
    }
    
    public static IEndpointRouteBuilder MapGraphQlApi(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder
            .MapNitroApp(GraphQlConstants.NitroAppPath)
            .WithOptions(new GraphQLToolOptions()
            {
                DisableTelemetry = true,
                GaTrackingId = null
            });

        endpointRouteBuilder.MapGraphQL(GraphQlConstants.GraphQlPath);
        
        return endpointRouteBuilder;
    }
    
    public static IApplicationBuilder UseGraphQlStatusCodeMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<GraphQlStatusCodeMiddleware>();
        
        return app;
    }
}