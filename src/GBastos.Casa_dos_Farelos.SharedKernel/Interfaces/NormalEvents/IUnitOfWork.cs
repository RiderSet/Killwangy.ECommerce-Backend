namespace GBastos.Casa_dos_Farelos.SharedKernel.Interfaces.NormalEvents;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}