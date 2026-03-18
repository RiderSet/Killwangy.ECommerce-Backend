using GBastos.Casa_dos_Farelos.CadastroService.Application.DTOs;
using GBastos.Casa_dos_Farelos.CadastroService.Application.Queries.Veiculos.ObterVeiculo;
using GBastos.Casa_dos_Farelos.CadastroService.Domain.Aggregates;
using MediatR;

namespace GBastos.Casa_dos_Farelos.CadastroService.Application.Handlers;

public class ObterVeiculoQueryHandler : IRequestHandler<ObterVeiculoQuery, VeiculoDto?>
{
    private readonly IVeiculoRepository _veiculoRepository;

    public ObterVeiculoQueryHandler(IVeiculoRepository veiculoRepository)
    {
        _veiculoRepository = veiculoRepository;
    }

    public async Task<VeiculoDto?> Handle(ObterVeiculoQuery request, CancellationToken cancellationToken)
    {
        var veiculo = await _veiculoRepository.ObterPorPlacaAsync(request.Placa, cancellationToken);
        if (veiculo == null)
            return null;

        return new VeiculoDto
        {
            Id = veiculo.Id,
            Marca = veiculo.Marca,
            Modelo = veiculo.Modelo,
            Ano = veiculo.Ano,
            Cor = veiculo.Cor,
            Placa = veiculo.Placa,
            Tipo = veiculo.Tipo,
            Preco = veiculo.Preco,
            Disponivel = veiculo.Disponivel
        };
    }
}
