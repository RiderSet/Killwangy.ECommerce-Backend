namespace GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.MultiTenancy;

public interface ITenantEntity
{
    Guid TenantId { get; }
}