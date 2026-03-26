using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Common;
using GBastos.Casa_dos_Farelos.EstoqueService.Application.Commands;
using GBastos.Casa_dos_Farelos.EstoqueService.Application.Interfaces;
using MediatR;
using CadastroProduto = GBastos.Casa_dos_Farelos.CadastroService.Domain.Aggregates.Produto;

namespace GBastos.Casa_dos_Farelos.BuildingBlocks.Mapping.Handlers;

public class CriarProdutoHandler : IRequestHandler<CriarProdutoCommand, Result<Guid>>
{
    private readonly IEstoqueRepository _estoqueRepository;

    public CriarProdutoHandler(IEstoqueRepository estoqueRepository)
    {
        _estoqueRepository = estoqueRepository;
    }

    public async Task<Result<Guid>> Handle(CriarProdutoCommand command, CancellationToken cancellationToken)
    {
        var cadastroProduto = CadastroProduto.Criar(
            command.Nome,
            command.Descricao,
            command.PrecoVenda,
            command.PrecoCompra,
            command.QuantEstoque,
            command.CategoriaId
        );

        var estoqueProduto = ProdutoMapper.ToEstoqueProduto(cadastroProduto);

        await _estoqueRepository.AddAsync(estoqueProduto, cancellationToken);

        return Result<Guid>.Ok(cadastroProduto.Id);
    }
}