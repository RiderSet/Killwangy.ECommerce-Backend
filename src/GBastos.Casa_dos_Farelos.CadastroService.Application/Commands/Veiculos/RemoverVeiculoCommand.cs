using MediatR;

namespace GBastos.Casa_dos_Farelos.CadastroService.Application.Commands.Veiculos;

public sealed record RemoverVeiculoCommand(
    string Placa
) : IRequest<Unit>;