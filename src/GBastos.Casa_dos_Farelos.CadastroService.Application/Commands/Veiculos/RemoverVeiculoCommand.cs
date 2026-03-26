using MediatR;

namespace GBastos.Casa_dos_Farelos.CadastroService.Application.Commands.Veiculos;

public sealed record RemoverVeiculoCommand(
    Guid Id,
    string Placa
) : IRequest<Unit>
{
    public RemoverVeiculoCommand(Guid id)
        : this(id, string.Empty)
    {
    }

    public RemoverVeiculoCommand(string placa)
        : this(Guid.Empty, placa)
    {
    }
}