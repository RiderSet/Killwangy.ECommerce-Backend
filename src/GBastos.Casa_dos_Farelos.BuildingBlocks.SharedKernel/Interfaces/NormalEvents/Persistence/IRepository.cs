using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Abstractions;

namespace GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Interfaces.NormalEvents.Persistence;

public interface IRepository<T, TId>
    where T : AggregateRoot<TId>
    where TId : notnull
{
    Task<T?> GetByIdAsync(TId id);
    Task AddAsync(T entity);
    void Update(T entity);
    void Remove(T entity);
}