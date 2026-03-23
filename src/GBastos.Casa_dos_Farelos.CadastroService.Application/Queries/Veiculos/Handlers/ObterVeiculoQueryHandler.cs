using GBastos.Casa_dos_Farelos.CadastroService.Application.DTOs;
using GBastos.Casa_dos_Farelos.CadastroService.Application.Interfaces;
using GBastos.Casa_dos_Farelos.CadastroService.Application.Queries.Veiculos.ObterVeiculo;
using GBastos.Casa_dos_Farelos.CadastroService.Domain.Entities;
using GBastos.Casa_dos_Farelos.CadastroService.Domain.Enums;
using MediatR;

namespace GBastos.Casa_dos_Farelos.CadastroService.Application.Queries.Veiculos.Handlers;

public class ObterVeiculoQueryHandler : IRequestHandler<ObterVeiculoQuery, VeiculoDto?>
{
    private readonly IVeiculoRepository _veiculoRepository;

    public ObterVeiculoQueryHandler(IVeiculoRepository veiculoRepository)
    {
        _veiculoRepository = veiculoRepository;
    }

    public async Task<VeiculoDto?> Handle(ObterVeiculoQuery request, CancellationToken cancellationToken)
    {
        var veiculo = await _veiculoRepository.GetByPlacaAsync(request.Placa, cancellationToken);
        if (veiculo == null) return null;

        return new VeiculoDto
        {
            Id = veiculo.Id,
            Marca = veiculo.Marca ?? string.Empty,
            Modelo = veiculo.Modelo ?? string.Empty,
            AnoFabricacao = veiculo.AnoFabricacao,
            Cor = veiculo.Cor ?? string.Empty,
            Placa = veiculo.Placa ?? new PlacaVeiculo("XXX-0000"),
            Tipo = veiculo.Tipo ?? TipoVeiculo.Indefinido,
            ValorEstimado = veiculo.ValorEstimado,
            Disponivel = veiculo.Disponivel
        };
    }
}