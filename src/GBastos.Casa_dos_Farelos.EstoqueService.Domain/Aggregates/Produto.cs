using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Abstractions;
using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Exceptions;
using GBastos.Casa_dos_Farelos.EstoqueService.Domain.Entities;
using GBastos.Casa_dos_Farelos.EstoqueService.Domain.Events;

namespace GBastos.Casa_dos_Farelos.EstoqueService.Domain.Aggregates;

public class Produto : AggregateRoot<Guid>
{
    private readonly List<Reserva> _reservas = new();

    public string Nome { get; private set; } = string.Empty;
    public string Descricao { get; private set; } = string.Empty;
    public decimal PrecoVenda { get; private set; }
    public decimal PrecoCompra { get; private set; }
    public int QuantEstoque { get; private set; }
    public int QuantReservada => _reservas.Sum(x => x.Quantidade);
    public int QuantDisponivel => QuantEstoque - QuantReservada;
    public Guid CategoriaId { get; private set; }
    public Categoria Categoria { get; private set; } = null!;
    public byte[] RowVersion { get; private set; } = null!;

    public IReadOnlyCollection<Reserva> Reservas => _reservas;

    protected Produto() : base(Guid.Empty) { }

    public Produto(
        string nome,
        string descricao,
        decimal precoVenda,
        decimal precoCompra,
        Guid categoriaId,
        int estoqueInicial = 0)
            : base(Guid.NewGuid())
    {
        if (categoriaId == Guid.Empty)
            throw new DomainException("Categoria inválida.");

        if (estoqueInicial < 0)
            throw new DomainException("Estoque inicial inválido.");

        AlterarNome(nome);
        AlterarDescricao(descricao);
        AlterarPreco(precoVenda);

        CategoriaId = categoriaId;
        QuantEstoque = estoqueInicial;

        AddDomainEvent(
            new ProdutoCriadoEvent(Id, Nome, PrecoVenda));
    }

    public void EntradaEstoque(int quantidade)
    {
        if (quantidade <= 0)
            throw new DomainException("Quantidade inválida.");

        checked
        {
            QuantEstoque += quantidade;
        }

        AddDomainEvent(
            new EstoqueAtualizadoDomainEvent(
                Id,
                QuantEstoque));
    }

    public void SaidaEstoque(int quantidade)
    {
        if (quantidade <= 0)
            throw new DomainException("Quantidade inválida.");

        if (QuantDisponivel < quantidade)
            throw new DomainException(
                $"Estoque insuficiente para o produto {Nome}");

        QuantEstoque -= quantidade;

        AddDomainEvent(
            new EstoqueBaixadoDomainEvent(
                Id,
                Id,
                Nome,
                quantidade));
    }

    public Reserva ReservarEstoque(
        Guid produtoId,
        int quantidade,
        DateTime expiraEm)
    {
        if (produtoId == Guid.Empty)
            throw new DomainException("Pedido inválido.");

        if (quantidade <= 0)
            throw new DomainException("Quantidade inválida.");

        if (QuantDisponivel < quantidade)
            throw new DomainException(
                $"Estoque insuficiente para reservar {Nome}");

        var reserva = new Reserva(
            produtoId,
            quantidade,
            TimeSpan.FromMinutes(15));

        _reservas.Add(reserva);

        AddDomainEvent(
            new EstoqueReservadoDomainEvent(
                Id,
                produtoId,
                quantidade));

        return reserva;
    }

    public void ConfirmarReserva(Guid reservaId)
    {
        var reserva = _reservas
            .FirstOrDefault(x => x.Id == reservaId);

        if (reserva is null)
            throw new DomainException("Reserva não encontrada.");

        SaidaEstoque(reserva.Quantidade);

        _reservas.Remove(reserva);

        AddDomainEvent(
            new ReservaConfirmadaDomainEvent(
                Id,
                reservaId));
    }

    public void CancelarReserva(Guid reservaId)
    {
        var reserva = _reservas
            .FirstOrDefault(x => x.Id == reservaId);

        if (reserva is null)
            throw new DomainException("Reserva não encontrada.");

        _reservas.Remove(reserva);

        AddDomainEvent(
            new ReservaCanceladaDomainEvent(
                Id,
                reservaId));
    }

    public void Atualizar(
        string nome,
        string descricao,
        decimal preco,
        Guid categoriaId)
    {
        if (categoriaId == Guid.Empty)
            throw new DomainException("Categoria inválida.");

        AlterarNome(nome);
        AlterarDescricao(descricao);
        AlterarPreco(preco);

        CategoriaId = categoriaId;
    }

    private void AlterarNome(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new DomainException("Nome obrigatório.");

        Nome = nome.Trim();
    }

    private void AlterarDescricao(string descricao)
    {
        if (string.IsNullOrWhiteSpace(descricao))
            throw new DomainException("Descrição obrigatória.");

        Descricao = descricao.Trim();
    }

    private void AlterarPreco(decimal preco)
    {
        if (preco <= 0)
            throw new DomainException("Preço inválido.");

        PrecoVenda = preco;
    }

    protected override void ValidateInvariants()
    {
        if (Id == Guid.Empty)
            throw new DomainException("Id do produto inválido.");

        if (string.IsNullOrWhiteSpace(Nome))
            throw new DomainException("Nome obrigatório.");

        if (PrecoVenda <= 0)
            throw new DomainException("Preço inválido.");

        if (QuantEstoque < 0)
            throw new DomainException("Estoque inválido.");

        if (CategoriaId == Guid.Empty)
            throw new DomainException("Categoria inválida.");
    }
}