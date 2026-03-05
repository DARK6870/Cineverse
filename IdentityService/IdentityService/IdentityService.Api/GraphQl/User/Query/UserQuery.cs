using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using IdentityService.Application.Common.Models.Filters;
using IdentityService.Application.MediatR.Requests.Users.GetUserById;
using IdentityService.Application.MediatR.Requests.Users.GetUsers;
using IdentityService.Application.MediatR.Requests.Users.GetUsersDistinctFilterValues;
using IdentityService.Mongo.Schemas.Entities;
using Infrastructure.WebApi.GraphQl.Base;
using Infrastructure.WebApi.GraphQl.Constants;
using MediatR;

namespace IdentityService.Api.GraphQl.User.Query;

[ExtendObjectType(nameof(BaseGraphQlQuery))]
public class UserQuery
{
    [UseOffsetPaging(ProviderName = GraphQlConstants.QueryablePaginationProvider)]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public Task<IQueryable<UserEntity>> GetUsers(
        string? searchTerm,
        [Service] IMediator mediator,
        CancellationToken cancellationToken
    )
    {
        return mediator.Send(new GetUsersRequest(searchTerm), cancellationToken);
    }

    public async Task<UserEntity> GetUserById(
        string id,
        [Service] IMediator mediator,
        CancellationToken cancellationToken
    )
    {
        return await mediator.Send(new GetUserByIdRequest(id), cancellationToken);
    }

    public async Task<IEnumerable<string>> GetUsersDistinctFilterValues(
        UserFilterField field,
        [Service] IMediator mediator,
        CancellationToken cancellationToken
    )
    {
        return await mediator.Send(new GetUsersDistinctFilterValuesRequest(field), cancellationToken);
    }
}