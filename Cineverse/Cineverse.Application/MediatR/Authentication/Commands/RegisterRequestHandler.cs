using Cineverse.Infrastructure.Services.Interfaces;
using MediatR;

namespace Cineverse.Application.MediatR.Authentication.Commands;

public record RegisterRequest(
    string Email,
    string FirstName,
    string LastName,
    string Password,
    string ConfirmPassword
) : IRequest<bool>;

public class RegisterRequestHandler(
    IAuthenticationService authenticationService
) : IRequestHandler<RegisterRequest, bool>
{
    public async Task<bool> Handle(RegisterRequest request, CancellationToken cancellationToken)
    {
        await authenticationService.RegisterUserAsync(
            request.Email,
            request.FirstName,
            request.LastName,
            request.Password
        );

        return true;
    }
}