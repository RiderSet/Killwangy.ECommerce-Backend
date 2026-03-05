using GBastos.Casa_dos_Farelos.Messaging.Abstractions;
using GBastos.Casa_dos_Farelos.Shared.Interfaces;
using System.Text.Json;

namespace GBastos.Casa_dos_Farelos.Messaging.Serialization;

public sealed class SystemTextJsonEventSerializer : IEventSerializer
{
    public string Serialize(IIntegrationEvent @event)
        => JsonSerializer.Serialize(@event, @event.GetType());

    public IIntegrationEvent Deserialize(string payload, string eventType)
    {
        var type = Type.GetType(eventType)!;
        return (IIntegrationEvent)JsonSerializer.Deserialize(payload, type)!;
    }
}