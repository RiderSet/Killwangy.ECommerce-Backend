using GBastos.Casa_dos_Farelos.FaturamentoService.Application.DTOs;
using MediatR;

namespace GBastos.Casa_dos_Farelos.FaturamentoService.Application.Queries.ObtertFaturas;

public record ObterFaturaPorIdQuery(Guid Id)
    : IRequest<FaturaDto>;