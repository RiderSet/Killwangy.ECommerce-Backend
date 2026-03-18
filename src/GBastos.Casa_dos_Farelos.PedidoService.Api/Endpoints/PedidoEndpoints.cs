using GBastos.Casa_dos_Farelos.PedidoService.Api.Contracts;
using GBastos.Casa_dos_Farelos.PedidoService.Application.Commands.CriarPedido;
using GBastos.Casa_dos_Farelos.PedidoService.Application.ConfirmarPedido;

namespace GBastos.Casa_dos_Farelos.PedidoService.Api.Endpoints;

public static class PedidoEndpoints
{
    public static IEndpointRouteBuilder MapPedidoEndpoints(
        this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/pedido");

        group.MapPost("/criar", CriarPedidoAsync);
        group.MapGet("/health", Health);

        return endpoints;
    }

    private static async Task<IResult> CriarPedidoAsync(
        CriarPedidoRequest request,
        CriarPedidoHandler handler)
    {
        var command = new CriarPedidoCommand(request.ClienteId);

        var pedidoId = await handler.Handle(command);

        return Results.Created($"/pedido/{pedidoId}", new
        {
            PedidoId = pedidoId
        });
    }

    private static IResult Health()
    {
        return Results.Ok(new
        {
            Service = "PedidoService",
            Status = "Healthy",
            Timestamp = DateTime.UtcNow
        });
    }
}