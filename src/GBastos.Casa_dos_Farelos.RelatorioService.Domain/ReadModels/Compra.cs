using GBastos.Casa_dos_Farelos.RelatorioService.Domain.Common;

namespace GBastos.Casa_dos_Farelos.RelatorioService.Domain.ReadModels;

public class Compra : BaseEntity
{
    private readonly List<ItemCompra> _itens = new();
    public Guid FornecedorId { get; private set; }
    public string NomeFuncionario { get; private set; } = string.Empty;
    public DateTime DataCompra { get; private set; }
    public decimal Total { get; private set; }
    public IReadOnlyCollection<ItemCompra> Itens => _itens;

    protected Compra() { }

    public Compra(Guid id, Guid fornecedorId, string nomeFuncionario, DateTime data)
    {
        Id = id;
        FornecedorId = fornecedorId;
        NomeFuncionario = nomeFuncionario;
        DataCompra = data;
    }

    public void AdicionarItem(ItemCompra item)
    {
        _itens.Add(item);
        Total = _itens.Sum(x => x.SubTotal);
    }
}