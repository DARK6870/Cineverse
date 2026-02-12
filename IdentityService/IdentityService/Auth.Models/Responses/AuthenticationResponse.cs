namespace Auth.Models.Responses;

public record AuthenticationResponse(
    string RefreshToken,
    string AccessToken,
    bool Success = true,
    string? Message = null
);