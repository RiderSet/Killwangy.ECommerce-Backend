using GBastos.Casa_dos_Farelos.AuthService.Application.Queries.Responses;
using MediatR;

namespace GBastos.Casa_dos_Farelos.AuthService.Application.Commands.LoginUser;

public sealed record LoginUserCommand(
    string Email,
    string Password
) : IRequest<LoginUserResponse>;