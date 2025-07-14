using Cineverse.Infrastructure.Common.Models;
using Cineverse.Infrastructure.Services.Interfaces;
using MediatR;

namespace Cineverse.Application.MediatR.Authentication.Commands;

public record LoginRequest(
    string Email,
    string Password
) : IRequest<LoginResponse>;

public class LoginRequestHandler(
    IAuthenticationService authenticationService
) : IRequestHandler<LoginRequest, LoginResponse>
{
    public async Task<LoginResponse> Handle(LoginRequest request, CancellationToken cancellationToken)
    {
        return await authenticationService.LoginUserAsync(request.Email, request.Password);
    }
}