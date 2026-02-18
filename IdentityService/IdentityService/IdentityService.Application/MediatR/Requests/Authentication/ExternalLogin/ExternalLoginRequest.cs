using IdentityService.Mongo.Schemas.Enums;
using MediatR;

namespace IdentityService.Application.MediatR.Requests.Authentication.ExternalLogin;

public record ExternalLoginRequest(
    AuthenticationProvider Provider
) : IRequest;