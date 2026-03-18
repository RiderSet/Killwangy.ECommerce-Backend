using GBastos.Casa_dos_Farelos.AuthService.Application.Queries.Responses;
using MediatR;

namespace GBastos.Casa_dos_Farelos.AuthService.Application.Commands.RefreshToken;

public sealed record RefreshTokenCommand(
    string RefreshToken
) : IRequest<RefreshTokenResponse>;