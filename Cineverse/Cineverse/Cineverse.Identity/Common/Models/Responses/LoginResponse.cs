namespace Cineverse.Infrastructure.Common.Models;

public record AuthenticationResponse(
    string RefreshToken,
    string AccessToken,
    bool Success = true,
    string? Message = null
);