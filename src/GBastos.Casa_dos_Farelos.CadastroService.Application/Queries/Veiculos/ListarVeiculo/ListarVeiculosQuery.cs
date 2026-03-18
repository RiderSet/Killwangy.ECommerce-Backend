using GBastos.Casa_dos_Farelos.CadastroService.Application.DTOs;
using MediatR;

namespace GBastos.Casa_dos_Farelos.CadastroService.Application.Queries.Veiculos.ListarVeiculo;

public sealed record ListarVeiculosQuery
    : IRequest<List<VeiculoDto>>;