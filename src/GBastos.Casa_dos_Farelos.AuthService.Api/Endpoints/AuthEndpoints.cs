using GBastos.Casa_dos_Farelos.AuthService.Application.Services;
using GBastos.Casa_dos_Farelos.AuthService.Domain.Entities;

namespace GBastos.Casa_dos_Farelos.AuthService.Infrastructure.Security;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this WebApplication app)
    {
        app.MapPost("/login", (
            LoginRequest request,
            JwtTokenService jwtService) =>
        {
            // simulação de validação
            if (request.Email != "admin@teste.com")
                return Results.Unauthorized();

            var user = new User(
                request.Email,
                "fake-password-hash",
                "User"
            );

            // tenant FIXO (depois vem do banco)
            var tenant = new Tenant
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111")
            };

            var token = jwtService.GenerateToken(user, tenant);

            return Results.Ok(new
            {
                access_token = token
            });
        });
    }
}

public record LoginRequest(string Email, string Password);