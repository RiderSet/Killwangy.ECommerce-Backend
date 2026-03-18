namespace GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.ValueObjects;

public abstract class ValueObject
{
    protected abstract IEnumerable<object> GetEqualityComponents();
}