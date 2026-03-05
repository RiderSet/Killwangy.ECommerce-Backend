using GBastos.Casa_dos_Farelos.PedidoService.Api.Contracts;
using GBastos.Casa_dos_Farelos.PedidoService.Domain.Domain.Events;
using GBastos.Casa_dos_Farelos.SharedKernel.Interfaces.NormalEvents;

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
        IEventBus eventBus)
    {
        var evento = new PedidoCriadoEvent(
            Guid.NewGuid(),
            request.ClienteId,
            request.Valor
        );

        await eventBus.PublishAsync(evento);

        return Results.Ok(new
        {
            evento.PedidoId,
            evento.Valor
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