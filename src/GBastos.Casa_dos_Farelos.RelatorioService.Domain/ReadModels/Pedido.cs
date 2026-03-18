using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Abstractions;

namespace GBastos.Casa_dos_Farelos.RelatorioService.Domain.ReadModels;

public class Pedido : BaseEntity
{
    private readonly List<ItemVenda> _itens = new();

    public Guid ClienteId { get; private set; }
    public string NomeCliente { get; private set; } = string.Empty;

    public DateTime Data { get; private set; }

    public decimal Total { get; private set; }

    public IReadOnlyCollection<ItemVenda> Itens => _itens;

    protected Pedido() { }

    public Pedido(Guid id, Guid clienteId, string nomeCliente, DateTime data)
    {
        Id = id;
        ClienteId = clienteId;
        NomeCliente = nomeCliente;
        Data = data;
    }

    public void AdicionarItem(ItemVenda item)
    {
        _itens.Add(item);
        RecalcularTotal();
    }

    private void RecalcularTotal()
    {
        Total = _itens.Sum(x => x.SubTotal);
    }
}