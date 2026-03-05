using GBastos.Casa_dos_Farelos.PedidoService.Domain.Domain.Events;
using GBastos.Casa_dos_Farelos.PedidoService.Domain.Entities;
using GBastos.Casa_dos_Farelos.PedidoService.Domain.Enums;
using GBastos.Casa_dos_Farelos.SharedKernel.Common;
using GBastos.Casa_dos_Farelos.SharedKernel.Exceptions;

namespace GBastos.Casa_dos_Farelos.PedidoService.Domain.Aggregates;

public class Pedido : AggregateRoot<Guid>
{
    private readonly List<ItemPedido> _itens = new();

    public Guid ClienteId { get; private set; }
    public StatusPedido Status { get; private set; }
    public Money? Total { get; private set; }

    public IReadOnlyCollection<ItemPedido> Itens => _itens;

    protected Pedido() : base(Guid.Empty) { } 

    private Pedido(Guid clienteId)
        : base(Guid.NewGuid())
    {
        if (clienteId == Guid.Empty)
            throw new DomainException("Cliente inválido.");

        ClienteId = clienteId;
        Status = StatusPedido.Criado;
        Total = Money.Zero("BRL");
    }

    public static Pedido Criar(Guid clienteId)
    {
        var pedido = new Pedido(clienteId);

        pedido.AddDomainEvent(
            new PedidoCriadoEvent(
                pedido.Id,
                pedido.ClienteId
            ));

        return pedido;
    }

    public void AdicionarItem(
        Guid produtoId,
        string nomeProduto,
        int quantidade,
        decimal precoUnitario)
    {
        if (Status != StatusPedido.Criado)
            throw new DomainException("Não é possível alterar o pedido.");

        if (produtoId == Guid.Empty)
            throw new DomainException("Produto inválido.");

        if (string.IsNullOrWhiteSpace(nomeProduto))
            throw new DomainException("Nome do produto obrigatório.");

        if (quantidade <= 0)
            throw new DomainException("Quantidade inválida.");

        if (precoUnitario <= 0)
            throw new DomainException("Preço inválido.");

        var item = new ItemPedido(
            Id,
            produtoId,
            nomeProduto,
            quantidade,
            precoUnitario);

        _itens.Add(item);

        RecalcularTotal();
    }

    public void Confirmar()
    {
        if (!_itens.Any())
            throw new DomainException("Pedido sem itens.");

        if (Status != StatusPedido.Criado)
            throw new DomainException("Pedido já processado.");

        if (Total is not Money total || total.Amount <= 0)
            throw new DomainException("Pedido com valor inválido.");

        Status = StatusPedido.Confirmado;

        AddDomainEvent(
            new PedidoConfirmadoEvent(
                Id,
                ClienteId,
                Total
            ));
    }

    public void Cancelar()
    {
        if (Status == StatusPedido.Pago)
            throw new DomainException("Não pode cancelar pedido pago.");

        Status = StatusPedido.Cancelado;

        AddDomainEvent(
            new PedidoCanceladoEvent(
                Id,
                ClienteId
            ));
    }

    private void RecalcularTotal()
    {
        var totalAmount = _itens.Sum(i => i.SubTotal);
        Total = new Money(totalAmount, "BRL");
    }

    protected override void ValidateInvariants()
    {
        if (Id == Guid.Empty)
            throw new DomainException("Id do pedido inválido.");

        if (ClienteId == Guid.Empty)
            throw new DomainException("Cliente inválido.");

        if (!Enum.IsDefined(typeof(StatusPedido), Status))
            throw new DomainException("Status do pedido inválido.");

        if (_itens.Any())
        {
            var somaItens = _itens.Sum(i => i.SubTotal);

            if (Total is null)
                throw new DomainException("Total não pode ser nulo quando há itens.");

            if (Total.Amount != somaItens)
                throw new DomainException("Total do pedido inconsistente.");
        }

        if ((Status == StatusPedido.Confirmado || Status == StatusPedido.Pago)
            && !_itens.Any())
            throw new DomainException("Pedido confirmado deve possuir itens.");

        if (Total is not null && Total.Amount < 0)
            throw new DomainException("Total do pedido não pode ser negativo.");
    }
}