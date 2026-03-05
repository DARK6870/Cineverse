using Auth.Models.Enums;
using MediatR;

namespace IdentityService.Application.MediatR.Requests.Users.UpdateUser;

public record UpdateUserRequest(
    string Id,
    Role Role,
    UserStatus Status
) : IRequest<bool>;