using GBastos.Casa_dos_Farelos.AuthService.Application.Commands.Login;
using GBastos.Casa_dos_Farelos.AuthService.Application.Commands.RefreshToken;
using GBastos.Casa_dos_Farelos.AuthService.Application.Commands.RegisterUser;
using MediatR;

namespace GBastos.Casa_dos_Farelos.AuthService.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/auth")
            .WithTags("Auth");

        group.MapPost("/login", Login);
        group.MapPost("/register", Register);
        group.MapPost("/refresh-token", async (
            RefreshTokenCommand command,
            IMediator mediator) =>
                {
                    var result = await mediator.Send(command);
                    return Results.Ok(result);
                });
    }

    private static async Task<IResult> Login(
        LoginUserCommand command,
        IMediator mediator)
            {
                var result = await mediator.Send(command);
                return Results.Ok(result);
            }

    private static async Task<IResult> Register(
        RegisterUserCommand command,
        IMediator mediator)
            {
                var result = await mediator.Send(command);
                return Results.Ok(result);
            }
}