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

        // 5️⃣ Opcional: disparar Domain Event (já feito pelo Aggregate)
        // veiculo.AddDomainEvent(new VeiculoCriadoEvent(veiculo.Id, veiculo.Placa.Valor));

        return Unit.Value;
    }
}