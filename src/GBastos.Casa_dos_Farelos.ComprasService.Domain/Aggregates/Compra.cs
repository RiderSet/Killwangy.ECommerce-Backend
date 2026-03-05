using GBastos.Casa_dos_Farelos.ComprasService.Domain.Entities;
using GBastos.Casa_dos_Farelos.ComprasService.Domain.Events;
using GBastos.Casa_dos_Farelos.SharedKernel.Common;
using GBastos.Casa_dos_Farelos.SharedKernel.Exceptions;

namespace GBastos.Casa_dos_Farelos.ComprasService.Domain.Aggregates;

public class Compra : AggregateRoot<Guid>
{
    private readonly List<ItemCompra> _itens = new();
    public StatusCompra Status { get; private set; }
    public IReadOnlyCollection<ItemCompra> Itens => _itens;
    protected Compra() : base(Guid.Empty) { }

    public Compra(Guid id) : base(id)
    {
        if (id == Guid.Empty)
            throw new DomainException("Id inválido.");

        Status = StatusCompra.Criada;
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
                    item.Quantidade
                ));
        }
    }

    public void Cancelar()
    {
        if (Status == StatusCompra.Finalizada)
            throw new DomainException("Não é possível cancelar compra finalizada.");

        Status = StatusCompra.Cancelada;
    }

    protected override void ValidateInvariants()
    {
        if (Id == Guid.Empty)
            throw new DomainException("Id da compra inválido.");

        if (!Enum.IsDefined(typeof(StatusCompra), Status))
            throw new DomainException("Status da compra inválido.");

        if (_itens is null)
            throw new DomainException("Lista de itens inválida.");

        if (_itens.Any(i => i == null))
            throw new DomainException("Compra contém item inválido.");

        if (Status == StatusCompra.Finalizada && !_itens.Any())
            throw new DomainException("Compra finalizada deve possuir itens.");

        if (_itens.Any(i => i.Quantidade <= 0))
            throw new DomainException("Item com quantidade inválida.");

        if (_itens.Any(i => i.CustoUnitario.Amount <= 0))
            throw new DomainException("Item com custo inválido.");
    }

    public void AlterarQuantidadeItem(Guid itemId, int quantidade)
    {
        var item = _itens.FirstOrDefault(x => x.Id == itemId);

        if (item is null)
            throw new DomainException("Item não encontrado.");

        item.AlterarQuantidade(quantidade);
    }
}