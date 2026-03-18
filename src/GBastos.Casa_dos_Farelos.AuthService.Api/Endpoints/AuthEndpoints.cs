using GBastos.Casa_dos_Farelos.AuthService.Application.Services;
using GBastos.Casa_dos_Farelos.AuthService.Domain.Entities;
using Microsoft.AspNetCore.Identity.Data;

namespace GBastos.Casa_dos_Farelos.AuthService.Api.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this WebApplication app)
    {
        app.MapPost("/login", (
            LoginRequest request,
            JwtTokenService jwtService) =>
        {
            var user = new User(
                request.Email,
                "fake-password-hash",
                "User"
            );

            var tenant = new Tenant
            {
                Id = Guid.NewGuid()
            };

            var token = jwtService.GenerateToken(user, tenant);

            return Results.Ok(new
            {
                access_token = token
            });
        });
    }
}