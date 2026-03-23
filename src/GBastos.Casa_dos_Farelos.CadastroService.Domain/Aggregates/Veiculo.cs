using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Abstractions;
using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.DomainEvents;
using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Exceptions;
using GBastos.Casa_dos_Farelos.CadastroService.Domain.Entities;
using GBastos.Casa_dos_Farelos.CadastroService.Domain.Enums;

namespace GBastos.Casa_dos_Farelos.CadastroService.Domain.Aggregates;

public class Veiculo : AggregateRoot<Guid>
{
    public string Marca { get; set; } = string.Empty;
    public string? Modelo { get; private set; }
    public int AnoFabricacao { get; set; }
    public string? Cor { get; set; } = string.Empty;
    public PlacaVeiculo Placa { get; set; } = default!;
    public TipoVeiculo? Tipo { get; private set; }
    public decimal ValorEstimado { get; set; }
    public Guid? ProprietarioId { get; private set; }

    public bool Disponivel { get; set; } = true;

    private Veiculo() : base(Guid.Empty) { }

    private Veiculo(Guid id, PlacaVeiculo placa, string? modelo, TipoVeiculo tipo, Guid? proprietarioId)
        : base(id)
    {
        Placa = placa;
        Modelo = modelo;
        Tipo = tipo;
        ProprietarioId = proprietarioId;

        ValidateInvariants();

        AddDomainEvent(new VeiculoCriadoEvent(Id, placa.Valor));
    }

    public static Veiculo Criar(string placa, string? modelo, TipoVeiculo tipo, Guid? proprietarioId = null)
    {
        if (string.IsNullOrWhiteSpace(placa))
            throw new DomainException("Placa é obrigatória.");

        if (tipo == default)
            throw new DomainException("Tipo é obrigatório.");

        return new Veiculo(Guid.NewGuid(), new PlacaVeiculo(placa), modelo, tipo, proprietarioId);
    }

    protected override void ValidateInvariants()
    {
        if (Placa is null)
            throw new DomainException("Placa inválida.");

        if (Tipo == default)
            throw new DomainException("Tipo de veículo inválido.");
    }
}