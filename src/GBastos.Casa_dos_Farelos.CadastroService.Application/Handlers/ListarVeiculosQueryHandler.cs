using GBastos.Casa_dos_Farelos.CadastroService.Application.DTOs;
using GBastos.Casa_dos_Farelos.CadastroService.Application.Queries.Veiculos.ListarVeiculo;
using GBastos.Casa_dos_Farelos.CadastroService.Domain.Entities;
using GBastos.Casa_dos_Farelos.CadastroService.Domain.Enums;
using GBastos.Casa_dos_Farelos.CadastroService.Infrastructure.Persistence.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GBastos.Casa_dos_Farelos.CadastroService.Application.Handlers;

public class ListarVeiculosQueryHandler
    : IRequestHandler<ListarVeiculosQuery, List<VeiculoDto>>
{
    private readonly CadastroDbContext _context;

    public ListarVeiculosQueryHandler(CadastroDbContext context)
    {
        _context = context;
    }

    public async Task<List<VeiculoDto>> Handle(
        ListarVeiculosQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.Veiculos
            .Select(x => new VeiculoDto
            {
                Id = x.Id,
                Marca = x.Marca ?? string.Empty,
                Modelo = x.Modelo ?? string.Empty,
                AnoFabricacao = x.AnoFabricacao,
                Cor = x.Cor ?? string.Empty,
                Placa = x.Placa ?? new PlacaVeiculo("XXX-0000"),
                Tipo = x.Tipo ?? TipoVeiculo.ProprioEmpresa,
                ValorEstimado = x.ValorEstimado,
                Disponivel = x.Disponivel
            })
            .ToListAsync(cancellationToken);
    }
}