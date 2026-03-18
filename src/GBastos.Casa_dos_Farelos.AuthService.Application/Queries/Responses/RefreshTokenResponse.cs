namespace GBastos.Casa_dos_Farelos.AuthService.Application.Queries.Responses;

public sealed record RefreshTokenResponse(
    string AccessToken,
    string RefreshToken,
    DateTime ExpiresAt
);