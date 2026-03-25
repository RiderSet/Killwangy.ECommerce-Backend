using GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Abstractions;
using GBastos.Casa_dos_Farelos.FaturamentoService.Domain.Entities;
using GBastos.Casa_dos_Farelos.FaturamentoService.Domain.Enums;
using GBastos.Casa_dos_Farelos.FaturamentoService.Domain.Events.Publish;
using GBastos.Casa_dos_Farelos.FaturamentoService.Domain.Events.PublishEvents;

namespace GBastos.Casa_dos_Farelos.FaturamentoService.Domain.Aggregates;

public class Fatura : AggregateRoot<Guid>
{
    private readonly List<ItemFatura> _itens = new();

    public string Numero { get; private set; } = default!;
    public string Motivo { get; private set; } = default!;
    public Guid ClienteId { get; private set; }
    public Guid PedidoId { get; private set; }
    public DateTime DataEmissao { get; private set; }
    public DateTime DataVencimento { get; private set; }
    public StatusFatura Status { get; private set; }
    public decimal ValorTotal => _itens.Sum(x => x.Total);

    public IReadOnlyCollection<ItemFatura> Itens => _itens.AsReadOnly();

    // Construtor EF Core
    private Fatura() : base(Guid.NewGuid()) { }

    // Construtor principal
    public Fatura(string numero, Guid clienteId, Guid pedidoId, DateTime dataVencimento)
        : base(Guid.NewGuid())
    {
        Numero = numero ?? throw new ArgumentNullException(nameof(numero));
        ClienteId = clienteId;
        PedidoId = pedidoId;
        DataEmissao = DateTime.UtcNow;
        DataVencimento = dataVencimento;
        Status = StatusFatura.Rascunho;

        ValidateInvariants();
    }

    public Fatura(string numero)
        : base(Guid.NewGuid())
    {
        Numero = numero ?? throw new ArgumentNullException(nameof(numero));
        DataEmissao = DateTime.UtcNow;
        Status = StatusFatura.Rascunho;
    }

    // Atualiza a data de vencimento (só fatura em rascunho)
    public void AtualizarDataVencimento(DateTime novaData)
    {
        if (Status != StatusFatura.Rascunho)
            throw new InvalidOperationException("Só é possível atualizar faturas em Rascunho.");

        DataVencimento = novaData;
        ValidateInvariants();
    }

    // Adiciona item
    public void AdicionarItem(Guid produtoId, int quantidade, decimal valorUnitario)
    {
        if (Status != StatusFatura.Rascunho)
            throw new InvalidOperationException("Não é possível alterar uma fatura emitida.");

        _itens.Add(new ItemFatura(produtoId, quantidade, valorUnitario));
        ValidateInvariants();
    }

    // Emite a fatura
    public void Emitir()
    {
        if (!_itens.Any())
            throw new InvalidOperationException("Fatura deve possuir itens.");

        if (Status != StatusFatura.Rascunho)
            throw new InvalidOperationException("Fatura já foi emitida ou cancelada.");

        Status = StatusFatura.Emitida;

        AddDomainEvent(new FaturaEmitidaEvent(Id, Numero, ValorTotal, DateTime.UtcNow));
        ValidateInvariants();
    }
        
    // Cancela a fatura
    public void Cancelar()
    {
        if (Status == StatusFatura.Cancelada)
            return;

        Status = StatusFatura.Cancelada;
        AddDomainEvent(new FaturaCanceladaEvent(Id, Numero, Motivo));

        ValidateInvariants();
    }

    // Marca como paga
    public void MarcarComoPaga()
    {
        if (Status != StatusFatura.Emitida)
            throw new InvalidOperationException("Somente faturas emitidas podem ser pagas.");

        Status = StatusFatura.Paga;
        AddDomainEvent(new FaturaPagaEvent(Id, Numero, DateTime.UtcNow));

        ValidateInvariants();
    }

    // Marca como vencida
    public void MarcarComoVencida()
    {
        if (Status != StatusFatura.Emitida)
            throw new InvalidOperationException("Somente faturas emitidas podem ser vencidas.");

        Status = StatusFatura.Vencida;
        AddDomainEvent(new FaturaVencidaEvent(Id, Numero, DateTime.UtcNow));

        ValidateInvariants();
    }

    // Remove todos os itens (se necessário)
    public void LimparItens()
    {
        if (Status != StatusFatura.Rascunho)
            throw new InvalidOperationException("Só é possível alterar uma fatura em Rascunho.");

        _itens.Clear();
        ValidateInvariants();
    }

    protected override void ValidateInvariants()
    {
        if (string.IsNullOrWhiteSpace(Numero))
            throw new InvalidOperationException("O número da fatura é obrigatório.");

        if (ClienteId == Guid.Empty)
            throw new InvalidOperationException("ClienteId é obrigatório.");

        if (PedidoId == Guid.Empty)
            throw new InvalidOperationException("PedidoId é obrigatório.");

        if (DataVencimento < DataEmissao)
            throw new InvalidOperationException("A data de vencimento não pode ser anterior à data de emissão.");

        if (_itens.Any(x => x.Quantidade <= 0 || x.ValorUnitario <= 0))
            throw new InvalidOperationException("Todos os itens da fatura devem ter quantidade e valor unitário maiores que zero.");
    }
}