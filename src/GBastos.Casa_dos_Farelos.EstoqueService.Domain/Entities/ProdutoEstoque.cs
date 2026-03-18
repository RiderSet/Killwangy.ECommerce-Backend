
using GBastos.Casa_dos_Farelos.EstoqueService.Domain.Common;

namespace GBastos.Casa_dos_Farelos.EstoqueService.Application.Interfaces;

public class ProdutoEstoque : BaseEntity
{
    public Guid ProdutoId { get; private set; }
    public int QuantidadeDisponivel { get; private set; }
    public int QuantidadeReservada { get; private set; }
    public DateTime? ReservaExpiraEmUtc { get; private set; }
    public DateTime AtualizadoEmUtc { get; private set; }
    public byte[] RowVersion { get; private set; } = default!;

    private ProdutoEstoque() { } // EF

    public ProdutoEstoque(Guid produtoId, int quantidadeInicial)
    {
        if (produtoId == Guid.Empty)
            throw new ArgumentException("ProdutoId inválido.");

        if (quantidadeInicial < 0)
            throw new ArgumentException("Quantidade inicial inválida.");

        Id = Guid.NewGuid();
        ProdutoId = produtoId;
        QuantidadeDisponivel = quantidadeInicial;
        AtualizadoEmUtc = DateTime.UtcNow;
    }

    public void Debitar(int quantidade)
    {
        if (quantidade <= 0)
            throw new ArgumentException("Quantidade inválida.");

        if (QuantidadeDisponivel < quantidade)
            throw new InvalidOperationException("Estoque insuficiente.");

        QuantidadeDisponivel -= quantidade;
        AtualizadoEmUtc = DateTime.UtcNow;
    }

    public void Creditar(int quantidade)
    {
        if (quantidade <= 0)
            throw new ArgumentException("Quantidade inválida.");

        QuantidadeDisponivel = checked(QuantidadeDisponivel + quantidade);
        AtualizadoEmUtc = DateTime.UtcNow;
    }

    public bool PossuiEstoque(int quantidade)
    {
        if (quantidade <= 0)
            return false;

        return QuantidadeDisponivel >= quantidade;
    }

    public void Reservar(int quantidade, TimeSpan tempoReserva)
    {
        if (!PossuiEstoque(quantidade))
            throw new InvalidOperationException("Estoque insuficiente.");

        QuantidadeDisponivel -= quantidade;
        QuantidadeReservada += quantidade;
        ReservaExpiraEmUtc = DateTime.UtcNow.Add(tempoReserva);
    }
}