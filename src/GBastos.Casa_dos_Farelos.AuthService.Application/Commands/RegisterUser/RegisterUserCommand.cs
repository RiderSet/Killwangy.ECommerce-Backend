using GBastos.Casa_dos_Farelos.AuthService.Application.Queries.Responses;
using MediatR;

namespace GBastos.Casa_dos_Farelos.AuthService.Application.Commands.RegisterUser;

public sealed record RegisterUserCommand(
    string Nome,
    string Email,
    string Password
) : IRequest<RegisterUserResponse>;