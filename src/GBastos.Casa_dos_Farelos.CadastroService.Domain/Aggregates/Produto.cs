using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Abstractions;
using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Exceptions;
using GBastos.Casa_dos_Farelos.CadastroService.Domain.Entities;

namespace GBastos.Casa_dos_Farelos.CadastroService.Domain.Aggregates;

public class Produto : AggregateRoot<Guid>
{
    public string Nome { get; private set; } = null!;
    public string DescricaoProduto { get; private set; } = null!;
    public decimal PrecoVenda { get; private set; }
    public int QuantEstoque { get; private set; }
    public Guid CategoriaId { get; private set; }

    public byte[] RowVersion { get; private set; } = null!;

    public Categoria? Categoria { get; private set; }

    public int QuantidadeDisponivel { get; set; }
    public int QuantidadeReservada { get; set; }

    public DateTime? ReservaExpiraEmUtc { get; set; }

    protected Produto() : base(Guid.Empty) { }

    public Produto(
        Guid id,
        string nome,
        string descricaoProduto,
        decimal precoVenda,
        int quantEstoque,
        Guid categoriaId) : base(id)
    {
        if (id == Guid.Empty)
            throw new DomainException("Produto inválido.");

        if (string.IsNullOrWhiteSpace(nome))
            throw new DomainException("Nome do produto é obrigatório.");

        if (string.IsNullOrWhiteSpace(descricaoProduto))
            throw new DomainException("Descrição do produto é obrigatória.");

        if (precoVenda < 0)
            throw new DomainException("Preço de venda não pode ser negativo.");

        if (quantEstoque < 0)
            throw new DomainException("Quantidade em estoque não pode ser negativa.");

        if (categoriaId == Guid.Empty)
            throw new DomainException("Categoria inválida.");

        Nome = nome;
        DescricaoProduto = descricaoProduto;
        PrecoVenda = precoVenda;
        QuantEstoque = quantEstoque;
        CategoriaId = categoriaId;

        ValidateInvariants();
    }

    public static Produto Criar(
        string nome,
        string descricaoProduto,
        decimal precoVenda,
        int quantEstoque,
        Guid categoriaId)
    {
        return new Produto(Guid.NewGuid(), nome, descricaoProduto, precoVenda, quantEstoque, categoriaId);
    }

    protected override void ValidateInvariants()
    {
        if (Id == Guid.Empty)
            throw new DomainException("Produto inválido.");

        if (string.IsNullOrWhiteSpace(Nome))
            throw new DomainException("Nome do produto inválido.");

        if (PrecoVenda < 0)
            throw new DomainException("Preço de venda inválido.");

        if (QuantEstoque < 0)
            throw new DomainException("Quantidade em estoque inválida.");

        if (CategoriaId == Guid.Empty)
            throw new DomainException("Categoria inválida.");
    }

    public void AtualizarPreco(decimal novoPreco)
    {
        if (novoPreco < 0)
            throw new DomainException("Preço não pode ser negativo.");

        PrecoVenda = novoPreco;
    }

    public void AjustarEstoque(int quantidade)
    {
        if (QuantEstoque + quantidade < 0)
            throw new DomainException("Estoque insuficiente para ajuste.");

        QuantEstoque += quantidade;
    }
}