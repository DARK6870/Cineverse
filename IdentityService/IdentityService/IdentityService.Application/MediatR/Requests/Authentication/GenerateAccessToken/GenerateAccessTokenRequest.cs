using Auth.Models.Responses;
using MediatR;

namespace IdentityService.Application.MediatR.Requests.Authentication.GenerateAccessToken;

public record GenerateAccessTokenRequest(string RefreshToken) : IRequest<AuthenticationResponse>;