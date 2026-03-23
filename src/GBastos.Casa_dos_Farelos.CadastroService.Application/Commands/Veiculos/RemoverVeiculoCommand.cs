using MediatR;

namespace GBastos.Casa_dos_Farelos.CadastroService.Application.Commands.Veiculos;

public sealed record RemoverVeiculoCommand(
    Guid Id,
    string Placa
) : IRequest<Unit>;