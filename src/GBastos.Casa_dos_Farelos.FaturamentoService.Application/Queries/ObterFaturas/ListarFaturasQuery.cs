using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Common;
using GBastos.Casa_dos_Farelos.FaturamentoService.Application.DTOs;
using MediatR;

namespace GBastos.Casa_dos_Farelos.FaturamentoService.Application.Queries.ObtertFaturas;

public record ListarFaturasQuery(
    int Page = 1,
    int PageSize = 10,
    Guid? ClienteId = null,
    DateTime? DataInicio = null,
    DateTime? DataFim = null,
    string? Status = null
) : IRequest<PagedResult<FaturaDto>>;