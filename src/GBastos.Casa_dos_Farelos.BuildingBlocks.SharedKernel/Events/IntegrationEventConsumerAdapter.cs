using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Interfaces.IntegrationEvents;
using MassTransit;

namespace GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Events;

public sealed class IntegrationEventConsumerAdapter<T, THandler> :
    IConsumer<T>
    where T : class, IIntegrationEvent
    where THandler : class, IIntegrationEventHandler<T>
{
    private readonly THandler _handler;

    public IntegrationEventConsumerAdapter(THandler handler)
    {
        _handler = handler;
    }

    public Task Consume(ConsumeContext<T> context)
    {
        return _handler.Handle(context.Message, context.CancellationToken);
    }
}