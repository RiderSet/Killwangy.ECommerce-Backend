using GBastos.Casa_dos_Farelos.CatalogoService.Application.Commands;
using GBastos.Casa_dos_Farelos.CatalogoService.Domain.Aggregates;
using GBastos.Casa_dos_Farelos.CatalogoService.Infrastructure.Interfaces;
using MediatR;

namespace GBastos.Casa_dos_Farelos.CatalogoService.Application.Handlers;

public class CriarProdutoHandler : IRequestHandler<CriarProdutoCommand, Guid>
{
    private readonly IProdutoRepository _repository;

    public CriarProdutoHandler(IProdutoRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(
        CriarProdutoCommand request,
        CancellationToken cancellationToken)
    {
        var produto = new Produto(
            request.Nome,
            request.Descricao,
            request.Preco);

        await _repository.AddAsync(produto);

        return produto.Id;
    }
}