using MediatR;

namespace GBastos.Casa_dos_Farelos.FaturamentoService.Domain.Events.Publish;

public record FaturaCanceladaEvent(
    Guid FaturaId,
    string Numero,
    string Motivo,
    Guid MessageId,
    DateTime OccurredOnUtc
) : INotification
{
    private DateTime utcNow;

    public FaturaCanceladaEvent(Guid faturaId, string numero, string motivo)
        : this(
            faturaId,
            numero,
            motivo,
            Guid.NewGuid(),
            DateTime.UtcNow
        )
    { }
}