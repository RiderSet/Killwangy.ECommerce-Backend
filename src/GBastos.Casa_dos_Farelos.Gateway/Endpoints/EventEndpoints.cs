using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Events;
using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Interfaces.NormalEvents;

namespace GBastos.Casa_dos_Farelos.Gateway.Endpoints;

public static class EventEndpoints
{
    public static IEndpointRouteBuilder MapEventEndpoints(
        this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints
            .MapGroup("/events")
            .WithTags("Events");

        group.MapPost("/publish", PublishAsync)
             .RequireAuthorization();

        group.MapPost("/subscribe", Subscribe)
             .RequireAuthorization();

        return endpoints;
    }

    private static async Task<IResult> PublishAsync(
        PublishEventRequest request,
        IEventBus eventBus,
        CancellationToken ct)
    {
        var integrationEvent = new GenericIntegrationEvent
        {
            EventType = request.EventType,
            Payload = request.Payload
            //,
            //OccurredOn = request.OccurredOnUtc
        };

        await eventBus.PublishAsync(integrationEvent, ct);

        return Results.Ok(new
        {
            message = "Evento publicado com sucesso",
            integrationEvent.Id,
            integrationEvent.EventType,
            integrationEvent.OccurredOnUtc
        });
    }

    private static IResult Subscribe(IEventBus eventBus)
    {
        eventBus.Subscribe<
            GenericIntegrationEvent,
            GenericIntegrationEventHandler>();

        return Results.Ok(new
        {
            message = "Subscription registrada com sucesso"
        });
    }
}