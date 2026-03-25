using FluentAssertions;
using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Exceptions;
using GBastos.Casa_dos_Farelos.EstoqueService.Application.Commands;
using GBastos.Casa_dos_Farelos.EstoqueService.Application.Handlers;
using GBastos.Casa_dos_Farelos.EstoqueService.Application.Interfaces;
using Moq;

namespace GBastos.Casa_dos_Farelos.Tests.Application.Unit.Produtos;

public class CriarProdutoPrecoInvalidoTests
{
    [Fact]
    public async Task Nao_deve_permitir_produto_com_preco_zero()
    {
        var repoMock = new Mock<IProdutoRepository>();
        var redisLockMock = new Mock<RedisLockHandle>();

        var handler = new CriarProdutoHandler(repoMock.Object, redisLockMock.Object);

        var categoriaId = Guid.NewGuid();
        var command = new CriarProdutoCommand(
            Nome: "Ração",
            Descricao: "Ração para cães",
            IdempotencyKey: null,
            PrecoVenda: 0m,
            PrecoCompra: 5m,
            CategoriaId: categoriaId,
            QuantEstoque: 5
        );

        // Act
        Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<DomainException>()
            .WithMessage("Preço inválido.");
    }
}
