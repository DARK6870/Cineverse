using MediatR;

namespace Cineverse.Application.MediatR.Requests.Authentication.DeleteRefreshToken;

public record DeleteRefreshTokenRequest : IRequest<bool>;