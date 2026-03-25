using GBastos.Casa_dos_Farelos.FaturamentoService.Application.DTOs;
using MediatR;

namespace GBastos.Casa_dos_Farelos.FaturamentoService.Application.Queries.GetFaturas;

public record GetFaturaByIdQuery(Guid Id)
    : IRequest<FaturaDto>;