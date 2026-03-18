namespace GBastos.Casa_dos_Farelos.AuthService.Application.Queries.Responses;

public sealed record RegisterUserResponse(
    Guid UserId,
    string Email
);