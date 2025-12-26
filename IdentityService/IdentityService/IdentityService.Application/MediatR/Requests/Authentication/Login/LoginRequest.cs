using Auth.Models.Responses;
using MediatR;

namespace IdentityService.Application.MediatR.Requests.Authentication.Login;

public record LoginRequest(
    string Email,
    string Password
) : IRequest<AuthenticationResponse>;