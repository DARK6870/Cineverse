using MediatR;

namespace Cineverse.Application.MediatR.Authentication.Commands;

public record RegisterRequest(
    string Email,
    string FirstName,
    string LastName,
    string Password,
    string ConfirmPassword
) : IRequest<bool>;

public class RegisterRequestHandler
{
    
}