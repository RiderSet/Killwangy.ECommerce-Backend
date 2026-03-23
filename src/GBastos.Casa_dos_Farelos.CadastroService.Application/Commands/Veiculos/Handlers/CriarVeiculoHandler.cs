using GBastos.Casa_dos_Farelos.CadastroService.Domain.Aggregates;
using GBastos.Casa_dos_Farelos.CadastroService.Infrastructure.Interfaces;
using MediatR;

namespace GBastos.Casa_dos_Farelos.CadastroService.Application.Commands.Veiculos.Handlers;

public sealed class CriarVeiculoHandler
    : IRequestHandler<CriarVeiculoCommand, Unit>
{
    private readonly IClienteRepository _clienteRepository;
    private readonly IVeiculoRepository _veiculoRepository;

    public CriarVeiculoHandler(
        IClienteRepository clienteRepository,
        IVeiculoRepository veiculoRepository)
    {
        _clienteRepository = clienteRepository;
        _veiculoRepository = veiculoRepository;
    }

    public async Task<Unit> Handle(
        CriarVeiculoCommand request,
        CancellationToken cancellationToken)
    {
        var cliente = await _clienteRepository.GetByIdAsync(request.ClienteId, cancellationToken);
        if (cliente == null)
            throw new KeyNotFoundException($"Cliente com ID {request.ClienteId} não encontrado.");

        var existente = await _veiculoRepository.GetByPlacaAsync(request.Placa, cancellationToken);
        if (existente != null)
            throw new InvalidOperationException($"Veículo com placa {request.Placa} já cadastrado.");

        var veiculo = Veiculo.Criar(
            request.Placa,
            request.Modelo,
            request.Tipo,
            request.ClienteId);

        await _veiculoRepository.AddAsync(veiculo, cancellationToken);

        // Retorna Unit porque handler não deve saber nada de HTTP
        return Unit.Value;
    }
}