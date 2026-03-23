using GBastos.Casa_dos_Farelos.CadastroService.Domain.Enums;
using MediatR;

namespace GBastos.Casa_dos_Farelos.CadastroService.Application.Commands.Veiculos;

public sealed record AtualizarVeiculoCommand(
    Guid Id,
    string Placa,
    string Modelo,
    TipoVeiculo Tipo,
    Guid ClienteId
) : IRequest<Unit>;