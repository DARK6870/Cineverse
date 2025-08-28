namespace Cineverse.Infrastructure.Common.Constants;

public static class GraphQlConstants
{
    public const string QueryablePaginationProvider = nameof(QueryablePaginationProvider);
    public const int DefaultPageSize = 25;
    public const int MaxPageSize = 250;

    public const string GraphQlPath = "/Api/graphql";
    public static string NitroAppPath => GraphQlPath + "/ui";
}