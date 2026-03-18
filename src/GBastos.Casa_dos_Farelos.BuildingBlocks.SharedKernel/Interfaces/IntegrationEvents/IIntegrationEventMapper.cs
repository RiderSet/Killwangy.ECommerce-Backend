using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Interfaces.NormalEvents;

namespace GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Interfaces.IntegrationEvents;

public interface IIntegrationEventMapper
{
    IIntegrationEvent? Map(IDomainEvent domainEvent);
}