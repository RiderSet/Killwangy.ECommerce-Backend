using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.IntegrationEvents.General;
using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Interfaces.NormalEvents;

namespace GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Interfaces.IntegrationEvents;

public interface IIntegrationEventRegistry
{
    IntegrationEvent? Map(IDomainEvent domainEvent);
}