using GBastos.Casa_dos_Farelos.FaturamentoService.Application.DTOs;
using GBastos.Casa_dos_Farelos.FaturamentoService.Application.Queries.GetFaturas;
using GBastos.Casa_dos_Farelos.FaturamentoService.Infrastructure.Persistence.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GBastos.Casa_dos_Farelos.FaturamentoService.Application.Handlers;

public class GetFaturaByIdQueryHandler
    : IRequestHandler<GetFaturaByIdQuery, FaturaDto?>
{
    private readonly FaturamentoDbContext _context;

    public GetFaturaByIdQueryHandler(FaturamentoDbContext context)
    {
        _context = context;
    }

    public async Task<FaturaDto?> Handle(
        GetFaturaByIdQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.Faturas
            .Where(x => x.Id == request.Id)
            .Select(x => new FaturaDto(
                x.Id,
                x.Numero,
                x.DataEmissao,
                x.Status.ToString(),
                x.ValorTotal,
                x.Itens.Select(i => new ItemFaturaDto(
                    i.ProdutoId,
                    i.Quantidade,
                    i.ValorUnitario,
                    i.Total
                )).ToList()
            ))
            .FirstOrDefaultAsync(cancellationToken);
    }
}