using GBastos.Casa_dos_Farelos.SharedKernel.Events;
using GBastos.Casa_dos_Farelos.SharedKernel.Interfaces.NormalEvents;

namespace GBastos.Casa_dos_Farelos.Gateway.Endpoints;

public static class EventEndpoints
{
    public static IEndpointRouteBuilder MapEventEndpoints(
        this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints
            .MapGroup("/events")
            .WithTags("Events");

        group.MapPost("/publish", PublishAsync);
        group.MapPost("/subscribe", Subscribe);

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
            Payload = request.Payload,
            OccurredOn = request.OccurredOnUtc
        };

        await eventBus.PublishAsync(integrationEvent, ct);

        return Results.Ok(new
        {
            Message = "Evento publicado com sucesso",
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
            Message = "Subscription registrada com sucesso"
        });
    }
}