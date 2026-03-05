using GBastos.Casa_dos_Farelos.SharedKernel.IntegrationEvents.General;
using GBastos.Casa_dos_Farelos.SharedKernel.Interfaces.NormalEvents;

namespace GBastos.Casa_dos_Farelos.SharedKernel.Interfaces.IntegrationEvents;

public interface IIntegrationEventRegistry
{
    IntegrationEvent? Map(IDomainEvent domainEvent);
}