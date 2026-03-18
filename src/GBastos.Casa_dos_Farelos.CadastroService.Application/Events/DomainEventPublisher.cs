using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.IntegrationEvents.Cadastros;
using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Interfaces.NormalEvents;
using GBastos.Casa_dos_Farelos.CadastroService.Domain.Events;
using MediatR;

namespace GBastos.Casa_dos_Farelos.CadastroService.Application.Events;

public sealed class DomainEventPublisher :
    INotificationHandler<UsuarioCriadoDomainEvent>
{
    private readonly IEventBus _eventBus;

    public DomainEventPublisher(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public async Task Handle(
        UsuarioCriadoDomainEvent notification,
        CancellationToken cancellationToken)
    {
        var integrationEvent = new UsuarioCriadoIntegrationEvent
        {
            UsuarioId = notification.UsuarioId,
            Nome = notification.Nome,
            Email = notification.Email
        };

        await _eventBus.PublishAsync(integrationEvent, cancellationToken);
    }
}