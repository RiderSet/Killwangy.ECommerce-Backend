using MediatR;

namespace GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.DomainEvents;

public sealed class VeiculoCriadoEvent : INotification
{
    public Guid VeiculoId { get; }
    public string Placa { get; }

    public VeiculoCriadoEvent(Guid veiculoId, string placa)
    {
        VeiculoId = veiculoId;
        Placa = placa;
    }
}