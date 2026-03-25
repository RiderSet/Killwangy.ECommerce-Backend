using GBastos.Casa_dos_Farelos.FaturamentoService.Domain.Entities;
using GBastos.Casa_dos_Farelos.FaturamentoService.Domain.Enums;
using GBastos.Casa_dos_Farelos.FaturamentoService.Domain.Events.Publish;
using MediatR;

namespace GBastos.Casa_dos_Farelos.FaturamentoService.Domain.Aggregates;

public class Fatura
{
    private readonly List<ItemFatura> _itens = new();
    private readonly List<INotification> _domainEvents = new();

    public Guid Id { get; private set; }
    public string Numero { get; private set; } = default!;
    public DateTime DataEmissao { get; private set; }
    public StatusFatura Status { get; private set; }
    public decimal ValorTotal => _itens.Sum(x => x.Total);

    public IReadOnlyCollection<ItemFatura> Itens => _itens;
    public IReadOnlyCollection<INotification> DomainEvents => _domainEvents;

    private Fatura() { }

    public Fatura(string numero)
    {
        Id = Guid.NewGuid();
        Numero = numero;
        DataEmissao = DateTime.UtcNow;
        Status = StatusFatura.Rascunho;
    }

    public void AdicionarItem(
        Guid produtoId,
        int quantidade,
        decimal valorUnitario)
    {
        if (Status != StatusFatura.Rascunho)
            throw new InvalidOperationException(
                "Não é possível alterar uma fatura emitida.");

        _itens.Add(new ItemFatura(
            produtoId,
            quantidade,
            valorUnitario));
    }

    public void Emitir()
    {
        if (!_itens.Any())
            throw new InvalidOperationException(
                "Fatura deve possuir itens.");

        Status = StatusFatura.Emitida;

        // exemplo futuro
        // AddDomainEvent(new FaturaEmitidaEvent(Id, Numero));
    }

    public void Cancelar()
    {
        if (Status == StatusFatura.Cancelada)
            return;

        Status = StatusFatura.Cancelada;

        AddDomainEvent(new FaturaCanceladaEvent(
            Id,
            Numero,
            DateTime.UtcNow));
    }

    private void AddDomainEvent(FaturaCanceladaEvent faturaCanceladaEvent)
    {
        throw new NotImplementedException();
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}