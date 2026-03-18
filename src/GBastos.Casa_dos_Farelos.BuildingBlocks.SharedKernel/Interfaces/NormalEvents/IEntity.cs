namespace GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Interfaces.NormalEvents;

public interface IEntity
{
    public object GetId() => Guid.NewGuid();
}
