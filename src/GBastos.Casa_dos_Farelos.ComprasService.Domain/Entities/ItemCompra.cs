using GBastos.Casa_dos_Farelos.SharedKernel.Abstractions;
using GBastos.Casa_dos_Farelos.SharedKernel.Common;
using GBastos.Casa_dos_Farelos.SharedKernel.Exceptions;

public class ItemCompra : Entity<Guid>
{
    public Guid CompraId { get; private set; }
    public Guid ProdutoId { get; private set; }
    public string NomeProduto { get; private set; } = null!;
    public int Quantidade { get; private set; }
    public Money CustoUnitario { get; private set; } = null!;
    public Money SubTotal => CustoUnitario * Quantidade;

    protected ItemCompra() : base(Guid.Empty) { }

    internal ItemCompra(
        Guid compraId,
        Guid produtoId,
        string nomeProduto,
        int quantidade,
        Money custoUnitario)
        : base(Guid.NewGuid())
    {
        if (compraId == Guid.Empty)
            throw new DomainException("Compra inválida.");

        if (produtoId == Guid.Empty)
            throw new DomainException("Produto inválido.");

        if (string.IsNullOrWhiteSpace(nomeProduto))
            throw new DomainException("Nome do produto é obrigatório.");

        if (quantidade <= 0)
            throw new DomainException("Quantidade deve ser maior que zero.");

        if (custoUnitario is null)
            throw new DomainException("Custo unitário inválido.");

        CompraId = compraId;
        ProdutoId = produtoId;
        NomeProduto = nomeProduto.Trim();
        Quantidade = quantidade;
        CustoUnitario = custoUnitario;
    }

    internal void AlterarQuantidade(int novaQuantidade)
    {
        if (novaQuantidade <= 0)
            throw new DomainException("Quantidade deve ser maior que zero.");

        Quantidade = novaQuantidade;
    }

    internal void AlterarCustoUnitario(Money novoCusto)
    {
        if (novoCusto is null)
            throw new DomainException("Custo inválido.");

        CustoUnitario = novoCusto;
    }

    internal void AlterarNomeProduto(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new DomainException("Nome do produto inválido.");

        NomeProduto = nome.Trim();
    }
}