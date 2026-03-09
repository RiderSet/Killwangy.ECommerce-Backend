using GBastos.Casa_dos_Farelos.EstoqueService.Application.Commands;
using GBastos.Casa_dos_Farelos.SharedKernel.Common;
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
        var produto = new Produto();

        _repo.Add(produto);

        await _repo.SaveChangesAsync(ct);

        return Result<Guid>.Ok(produto.Id);
    }
}