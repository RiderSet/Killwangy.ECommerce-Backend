using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Abstractions;
using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Common;
using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Exceptions;
using GBastos.Casa_dos_Farelos.ComprasService.Domain.Entities;
using GBastos.Casa_dos_Farelos.ComprasService.Domain.Events;

namespace GBastos.Casa_dos_Farelos.ComprasService.Domain.Aggregates;

public class Compra : AggregateRoot<Guid>
{
    private readonly List<ItemCompra> _itens = new();

    public Guid ClienteId { get; private set; }
    public StatusCompra Status { get; private set; }

    public IReadOnlyCollection<ItemCompra> Itens => _itens;

    protected Compra() : base(Guid.Empty) { }

    private Compra(Guid id, Guid clienteId) : base(id)
    {
        if (id == Guid.Empty)
            throw new DomainException("Id inválido.");

        if (clienteId == Guid.Empty)
            throw new DomainException("Cliente inválido.");

        ClienteId = clienteId;
        Status = StatusCompra.Criada;
    }

    public static Compra CriarCompra(Guid clienteId)
    {
        var compra = new Compra(Guid.NewGuid(), clienteId);

        compra.AddDomainEvent(
            new CompraCriadaDomainEvent(
                compra.Id,
                clienteId));

        return compra;
    }

    public void AdicionarItem(
        Guid produtoId,
        string nomeProduto,
        int quantidade,
        Money custoUnitario)
    {
        var item = new ItemCompra(
            Id,
            produtoId,
            nomeProduto,
            quantidade,
            custoUnitario);

        _itens.Add(item);
    }

    public void Finalizar()
    {
        if (!_itens.Any())
            throw new DomainException("Compra sem itens.");

        if (Status != StatusCompra.Criada)
            throw new DomainException("Compra já processada.");

        Status = StatusCompra.Finalizada;

        foreach (var item in _itens)
        {
            AddDomainEvent(
                new CompraFinalizadaDomainEvent(
                    Id,
                    item.ProdutoId,
                    item.NomeProduto,
                    item.Quantidade));
        }
    }

    public void Cancelar()
    {
        if (Status == StatusCompra.Finalizada)
            throw new DomainException("Não é possível cancelar compra finalizada.");

        Status = StatusCompra.Cancelada;
    }

    public void AlterarQuantidadeItem(Guid itemId, int quantidade)
    {
        var item = _itens.FirstOrDefault(x => x.Id == itemId);

        if (item is null)
            throw new DomainException("Item não encontrado.");

        item.AlterarQuantidade(quantidade);
    }

    protected override void ValidateInvariants()
    {
        if (Id == Guid.Empty)
            throw new DomainException("Id da compra inválido.");

        if (ClienteId == Guid.Empty)
            throw new DomainException("Cliente inválido.");

        if (!Enum.IsDefined(typeof(StatusCompra), Status))
            throw new DomainException("Status inválido.");

        if (_itens.Any(i => i.Quantidade <= 0))
            throw new DomainException("Quantidade inválida.");

        if (_itens.Any(i => i.CustoUnitario.Amount <= 0))
            throw new DomainException("Custo inválido.");
    }
}