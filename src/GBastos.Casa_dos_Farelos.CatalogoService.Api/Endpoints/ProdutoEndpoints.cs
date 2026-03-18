using GBastos.Casa_dos_Farelos.CatalogoService.Application.Commands;
using MediatR;

namespace GBastos.Casa_dos_Farelos.CatalogoService.Api.Endpoints;

public static class ProdutoEndpoints
{
    public static void MapProdutoEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/produtos", async (
            CriarProdutoCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var id = await mediator.Send(command, ct);
            return Results.Ok(id);
        });
    }
}