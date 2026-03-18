using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Abstractions;
using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Common;
using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Exceptions;
using GBastos.Casa_dos_Farelos.PedidoService.Domain.Domain.ValueObjects;
using GBastos.Casa_dos_Farelos.PedidoService.Domain.Entities;
using GBastos.Casa_dos_Farelos.PedidoService.Domain.Enums;
using GBastos.Casa_dos_Farelos.PedidoService.Domain.Events;

namespace GBastos.Casa_dos_Farelos.PedidoService.Domain.Aggregates;

public class Pedido : AggregateRoot<Guid>
{
    private readonly List<ItemPedido> _itens = new();

    public Guid ClienteId { get; private set; }
    public StatusPedido Status { get; private set; }
    public Money Total { get; private set; }
    public PedidoNumero Numero { get; private set; }

    public IReadOnlyCollection<ItemPedido> Itens => _itens.AsReadOnly();

    protected Pedido() : base(Guid.Empty)
    {
        Total = Money.Zero("BRL");
        Status = StatusPedido.Criado;
        Numero = new PedidoNumero(1);
        ClienteId = Guid.Empty;
    }

    private Pedido(Guid clienteId, PedidoNumero numero) : base(Guid.NewGuid())
    {
        if (clienteId == Guid.Empty)
            throw new DomainException("Cliente inválido.");

        ClienteId = clienteId;
        Status = StatusPedido.Criado;
        Total = Money.Zero("BRL");
        Numero = numero ?? throw new DomainException("Número do pedido inválido.");
    }

    public static Pedido Criar(Guid clienteId, int numeroPedido)
    {
        var pedido = new Pedido(clienteId, new PedidoNumero(numeroPedido));

        pedido.AddDomainEvent(new PedidoCriadoEvent(
            pedido.Id,
            pedido.ClienteId,
            pedido.Total,
            pedido.Numero
        ));

        return pedido;
    }

    public void AdicionarItem(
        Guid produtoId,
        string nomeProduto,
        int quantidade,
        Money precoUnitario)
    {
        if (Status != StatusPedido.Criado)
            throw new DomainException("Não é possível alterar o pedido.");

        var existente = _itens.FirstOrDefault(x => x.ProdutoId == produtoId);

        if (existente != null)
        {
            existente.AdicionarQuantidade(quantidade);
        }
        else
        {
            var item = new ItemPedido(
                Id,
                produtoId,
                nomeProduto,
                quantidade,
                precoUnitario,
                Numero);

            _itens.Add(item);
        }

        RecalcularTotal();
        ValidateInvariants();
    }

    public void Confirmar()
    {
        if (!_itens.Any())
            throw new DomainException("Pedido sem itens.");

        if (Status != StatusPedido.Criado)
            throw new DomainException("Pedido já processado.");

        if (Total.Amount <= 0)
            throw new DomainException("Pedido com valor inválido.");

        Status = StatusPedido.Confirmado;

        AddDomainEvent(new PedidoConfirmadoEvent(
            Id,
            ClienteId,
            Total,
            Numero
        ));
    }

    // Cancela pedido
    public void Cancelar()
    {
        if (Status == StatusPedido.Pago)
            throw new DomainException("Não pode cancelar pedido pago.");

        Status = StatusPedido.Cancelado;

        AddDomainEvent(new PedidoCanceladoEvent(
            Id,
            ClienteId,
            Numero
        ));
    }

    private void RecalcularTotal()
    {
        var totalAmount = _itens.Sum(i => i.SubTotal.Amount);
        var currency = _itens.FirstOrDefault()?.PrecoUnitario.Currency ?? "BRL";

        Total = new Money(totalAmount, currency);
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
            var somaItens = _itens.Sum(i => i.SubTotal.Amount);
            if (Total is null)
                throw new DomainException("Total não pode ser nulo quando há itens.");

            if (Total.Amount != somaItens)
                throw new DomainException("Total do pedido inconsistente.");
        }

        if ((Status == StatusPedido.Confirmado || Status == StatusPedido.Pago) && !_itens.Any())
            throw new DomainException("Pedido confirmado deve possuir itens.");

        if (Total != null && Total.Amount < 0)
            throw new DomainException("Total do pedido não pode ser negativo.");

        if (Numero == null)
            throw new DomainException("Número do pedido não pode ser nulo.");
    }

    public static Pedido Criar(Guid clienteId)
    {
        throw new NotImplementedException();
    }
}