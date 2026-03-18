using GBastos.Casa_dos_Farelos.CadastroService.Application.DTOs;
using MediatR;

namespace GBastos.Casa_dos_Farelos.CadastroService.Application.Queries.Veiculos.ObterVeiculo;

public sealed record ObterVeiculoQuery(
    string Placa
) : IRequest<VeiculoDto?>;