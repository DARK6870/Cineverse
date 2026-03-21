namespace AutomationTests.Models.Generic;

public record GraphQlPaginatedResponse<T>(
    T Items,
    PageInfo PageInfo,
    long TotalCount
);

public record PageInfo(bool HasNextPage, bool HasPreviousPage);