using GBastos.Casa_dos_Farelos.AuthService.Application.Queries.Responses;
using MediatR;

namespace GBastos.Casa_dos_Farelos.AuthService.Application.Commands.LoginUser;

public sealed class LoginUserCommandHandler
    : IRequestHandler<LoginUserCommand, LoginUserResponse>
{
    public async Task<LoginUserResponse> Handle(
        LoginUserCommand request,
        CancellationToken cancellationToken)
    {
        // Aqui normalmente você buscaria o usuário no banco
        // e validaria a senha

        var userId = Guid.NewGuid();

        var token = "fake-jwt-token"; // depois será gerado pelo JwtService

        return new LoginUserResponse(
            userId,
            request.Email,
            token,
            DateTime.UtcNow.AddHours(1)
        );
    }
}