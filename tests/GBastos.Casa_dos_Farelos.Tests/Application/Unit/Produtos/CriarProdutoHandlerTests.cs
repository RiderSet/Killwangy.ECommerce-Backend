using FluentAssertions;
using GBastos.Casa_dos_Farelos.CadastroService.Domain.Aggregates;
using GBastos.Casa_dos_Farelos.EstoqueService.Application.Commands;
using GBastos.Casa_dos_Farelos.EstoqueService.Application.Handlers;
using GBastos.Casa_dos_Farelos.EstoqueService.Application.Interfaces;
using Moq;

namespace GBastos.Casa_dos_Farelos.Tests.Application.Unit.Produtos;

public class CriarProdutoHandlerTests
{
    [Fact]
    public async Task Deve_criar_produto_com_sucesso()
    {
        var repoMock = new Mock<IProdutoRepository>();

        var redisLockMock = new Mock<RedisLockHandle>();

        var handler = new CriarProdutoHandler(repoMock.Object, redisLockMock.Object);

        var categoriaId = Guid.NewGuid();
        var command = new CriarProdutoCommand(
            "Ração",
            "Ração para cães",
            "Blá, blá, blá, blá, blá, blá, blá, blá, blá, blá, blá,...",
            10m,
            15m,
            categoriaId,
            5
        );

        // Act
        var id = await handler.Handle(command, CancellationToken.None);

        // Assert
        id.Should().NotBe(Guid.Empty);
        repoMock.Verify(r => r.AddAsync(It.IsAny<Produto>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}