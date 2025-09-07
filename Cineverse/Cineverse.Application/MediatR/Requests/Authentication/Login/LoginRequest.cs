using Cineverse.Infrastructure.Common.Models;
using MediatR;

namespace Cineverse.Application.MediatR.Requests.Authentication.Login;

public record LoginRequest(
    string Email,
    string Password
) : IRequest<AuthenticationResponse>;