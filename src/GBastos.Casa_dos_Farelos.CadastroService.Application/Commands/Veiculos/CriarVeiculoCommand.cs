using GBastos.Casa_dos_Farelos.CadastroService.Domain.Enums;
using MediatR;

namespace GBastos.Casa_dos_Farelos.CadastroService.Application.Commands.Veiculos;

public sealed class CriarVeiculoCommand : IRequest<Unit>
{
    public string Placa { get; }
    public string Modelo { get; }
    public TipoVeiculo Tipo { get; }
    public Guid ClienteId { get; }

    public CriarVeiculoCommand(
        string placa,
        string modelo,
        TipoVeiculo tipo,
        Guid clienteId)
    {
        Placa = placa;
        Modelo = modelo;
        Tipo = tipo;
        ClienteId = clienteId;
    }
}