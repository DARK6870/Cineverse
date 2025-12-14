namespace Infrastructure.WebApi.GraphQl.Constants;

public class GraphQlConstants
{
    public const string QueryablePaginationProvider = nameof(QueryablePaginationProvider);
    public const int DefaultPageSize = 25;
    public const int MaxPageSize = 250;

    public const string GraphQlPath = "/api/graphql";
    public static string NitroAppPath => GraphQlPath + "/ui";
}