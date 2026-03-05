using IdentityService.Application.Common.Models.Filters;
using MediatR;

namespace IdentityService.Application.MediatR.Requests.Users.GetUsersDistinctFilterValues;

public record GetUsersDistinctFilterValuesRequest(
    UserFilterField FilterField
) : IRequest<IEnumerable<string>>;