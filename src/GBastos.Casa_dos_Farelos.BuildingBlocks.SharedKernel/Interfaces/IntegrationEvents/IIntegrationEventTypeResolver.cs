namespace GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Interfaces.NormalEvents.Persistence;

public interface IIntegrationEventTypeResolver
{
    Type? Resolve(string eventName);
}