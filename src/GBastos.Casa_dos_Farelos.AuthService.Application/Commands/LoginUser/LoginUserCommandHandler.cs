using GBastos.Casa_dos_Farelos.AuthService.Application.Queries.Responses;
using MediatR;

namespace GBastos.Casa_dos_Farelos.AuthService.Application.Commands.LoginUser;

public sealed class LoginUserCommandHandler
    : IRequestHandler<LoginUserCommand, LoginUserResponse>
{
    public Task<LoginUserResponse> Handle(
        LoginUserCommand request,
        CancellationToken cancellationToken)
    {
        var userId = Guid.NewGuid();

        var token = "fake-jwt-token";

        var response = new LoginUserResponse(
            userId,
            request.Email,
            token,
            DateTime.UtcNow.AddHours(1)
        );

        return Task.FromResult(response);
    }
}