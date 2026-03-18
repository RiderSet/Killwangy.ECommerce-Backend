using GBastos.Casa_dos_Farelos.AuthService.Application.Queries.Responses;
using MediatR;

namespace GBastos.Casa_dos_Farelos.AuthService.Application.Commands.RefreshToken;

public sealed class RefreshTokenCommandHandler
    : IRequestHandler<RefreshTokenCommand, RefreshTokenResponse>
{
    public Task<RefreshTokenResponse> Handle(
        RefreshTokenCommand request,
        CancellationToken cancellationToken)
    {
        var newAccessToken = "new-access-token";
        var newRefreshToken = Guid.NewGuid().ToString();

        var response = new RefreshTokenResponse(
            newAccessToken,
            newRefreshToken,
            DateTime.UtcNow.AddHours(1)
        );

        return Task.FromResult(response);
    }
}