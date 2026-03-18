using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Abstractions;
using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Common;
using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Exceptions;
using GBastos.Casa_dos_Farelos.PedidoService.Domain.Domain.ValueObjects;

namespace GBastos.Casa_dos_Farelos.PedidoService.Domain.Entities;

public class ItemPedido : Entity<Guid>
{
    public Guid PedidoId { get; private set; }
    public Guid ProdutoId { get; private set; }

    public string NomeProduto { get; private set; }
    public PedidoNumero Numero { get; private set; }

    public int Quantidade { get; private set; }
    public Money PrecoUnitario { get; private set; }

    public Money SubTotal => new Money(Quantidade * PrecoUnitario.Amount, PrecoUnitario.Currency);

    protected ItemPedido() : base(Guid.Empty)
    {
        NomeProduto = string.Empty;
        PrecoUnitario = Money.Zero("BRL");
        Numero = new PedidoNumero(1);
        Quantidade = 0;
        PedidoId = Guid.Empty;
        ProdutoId = Guid.Empty;
    }

    public ItemPedido(
        Guid pedidoId,
        Guid produtoId,
        string nomeProduto,
        int quantidade,
        Money precoUnitario,
        PedidoNumero numero)
        : base(Guid.NewGuid())
    {
        if (pedidoId == Guid.Empty)
            throw new DomainException("Pedido inválido.");

        if (produtoId == Guid.Empty)
            throw new DomainException("Produto inválido.");

        if (string.IsNullOrWhiteSpace(nomeProduto))
            throw new DomainException("Nome do produto obrigatório.");

        if (quantidade <= 0)
            throw new DomainException("Quantidade inválida.");

        if (precoUnitario == null || precoUnitario.Amount <= 0)
            throw new DomainException("Preço inválido.");

        if (numero == null)
            throw new DomainException("Número do pedido inválido.");

        PedidoId = pedidoId;
        ProdutoId = produtoId;
        NomeProduto = nomeProduto;
        Quantidade = quantidade;
        PrecoUnitario = precoUnitario;
        Numero = numero;
    }

    public void AdicionarQuantidade(int quantidade)
    {
        if (quantidade <= 0)
            throw new DomainException("Quantidade inválida.");

        Quantidade += quantidade;
    }
}