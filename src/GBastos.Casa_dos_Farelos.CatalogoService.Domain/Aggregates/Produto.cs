using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Abstractions;
using GBastos.Casa_dos_Farelos.CatalogoService.Domain.Events;

namespace GBastos.Casa_dos_Farelos.CatalogoService.Domain.Aggregates;

public class Produto : AggregateRoot<Guid>
{
    public string Nome { get; private set; } = null!;
    public string Descricao { get; private set; } = null!;
    public decimal Preco { get; private set; }
    public bool Ativo { get; private set; }

    private Produto() : base(Guid.Empty) { } // EF

    public Produto(string nome, string descricao, decimal preco)
        : base(Guid.NewGuid())
    {
        Nome = nome;
        Descricao = descricao;
        Preco = preco;
        Ativo = true;

        ValidateInvariants();
        AddDomainEvent(new ProdutoCriadoDomainEvent(Id, Nome, Preco));
    }

    public void AtualizarPreco(decimal novoPreco)
    {
        Preco = novoPreco;

        ValidateInvariants();

        AddDomainEvent(new ProdutoPrecoAtualizadoDomainEvent(Id, Preco));
    }

    public void Desativar()
    {
        Ativo = false;
    }

    protected override void ValidateInvariants()
    {
        if (string.IsNullOrWhiteSpace(Nome))
            throw new ArgumentException("Nome obrigatório");

        if (Preco <= 0)
            throw new ArgumentException("Preço deve ser maior que zero");
    }
}