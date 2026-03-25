using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Exceptions;
using GBastos.Casa_dos_Farelos.CadastroService.Infrastructure.Interfaces;
using MediatR;

namespace GBastos.Casa_dos_Farelos.CadastroService.Application.Commands.Clientes.RemoverCliente;

public class RemoverClienteCommandHandler
    : IRequestHandler<RemoverClienteCommand, Unit>
{
    private readonly IClienteRepository _repository;

    public RemoverClienteCommandHandler(IClienteRepository repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(RemoverClienteCommand request, CancellationToken cancellationToken)
    {
        var cliente = await _repository.GetByIdAsync(request.Id, cancellationToken);

        if (cliente is null)
            throw new DomainException("Cliente não encontrado.");

        _repository.Remover(cliente);

        await _repository.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}