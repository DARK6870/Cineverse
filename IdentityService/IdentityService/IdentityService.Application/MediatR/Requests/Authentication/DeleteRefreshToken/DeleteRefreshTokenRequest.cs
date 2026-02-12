using MediatR;

namespace IdentityService.Application.MediatR.Requests.Authentication.DeleteRefreshToken;

public record DeleteRefreshTokenRequest : IRequest<bool>;