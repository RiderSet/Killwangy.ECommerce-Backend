using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Abstractions;
using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Exceptions;

namespace GBastos.Casa_dos_Farelos.RelatorioService.Domain.ReadModels;

public class VendasPorFuncionario : BaseEntity
{
    public Guid ClienteId { get; private set; }

    public Guid FuncionarioId { get; private set; }

    public DateTime DataVenda { get; private set; }

    private readonly List<ItemVenda> _itens = new();
    public IReadOnlyCollection<ItemVenda> Itens => _itens;

    public decimal TotalVenda => _itens.Sum(i => i.SubTotal);

    public decimal TaxaEntrega { get; private set; }

    protected VendasPorFuncionario() { } // EF

    private VendasPorFuncionario(Guid clienteId, Guid funcionarioId)
    {
        if (clienteId == Guid.Empty)
            throw new DomainException("Cliente inválido.");

        if (funcionarioId == Guid.Empty)
            throw new DomainException("Funcionário inválido.");

        ClienteId = clienteId;
        FuncionarioId = funcionarioId;
        DataVenda = DateTime.UtcNow;
    }

    public static VendasPorFuncionario Criar(Guid clienteId, Guid funcionarioId, List<ItemVenda> itens)
        => new VendasPorFuncionario(clienteId, funcionarioId);

    public void AdicionarItem(Guid produtoId, int quantidade, decimal precoUnitario)
    {
        if (quantidade <= 0)
            throw new DomainException("Quantidade inválida.");

        if (precoUnitario <= 0)
            throw new DomainException("Preço inválido.");

        var item = new ItemVenda(produtoId, quantidade, precoUnitario);
        item.DefinirVenda(Id);

        _itens.Add(item);
    }

    public void AplicarEntrega()
        => TaxaEntrega = TotalVenda * 0.10m;
}