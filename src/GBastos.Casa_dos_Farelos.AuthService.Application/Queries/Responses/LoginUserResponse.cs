namespace GBastos.Casa_dos_Farelos.AuthService.Application.Queries.Responses;

public sealed record LoginUserResponse(
    Guid UserId,
    string Email,
    string AccessToken,
    DateTime ExpiresAt
);