using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Common;
using GBastos.Casa_dos_Farelos.EstoqueService.Application.Commands;
using GBastos.Casa_dos_Farelos.EstoqueService.Application.Interfaces;
using GBastos.Casa_dos_Farelos.EstoqueService.Domain.Aggregates;
using MediatR;

namespace GBastos.Casa_dos_Farelos.EstoqueService.Application.Handlers;

public class CriarProdutoHandler
    : IRequestHandler<CriarProdutoCommand, Result<Guid>>
{
    private readonly IProdutoRepository _repo;

    public CriarProdutoHandler(IProdutoRepository repo)
    {
        _repo = repo;
    }

    public async Task<Result<Guid>> Handle(
        CriarProdutoCommand request,
        CancellationToken ct)
    {
        var produto = new Produto(
            request.Nome,
            request.Descricao,
            request.PrecoVenda,
            request.PrecoCompra,
            request.CategoriaId,
            request.QuantEstoque
        );

        _repo.Add(produto);

        await _repo.SaveChangesAsync(ct);

        return Result<Guid>.Ok(produto.Id);
    }
}